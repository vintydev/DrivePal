// Define map, circle, and markers variables outside of the DOMContentLoaded event listener
let map;
let circle;
let markers = [];

document.addEventListener("DOMContentLoaded", async function () {
    // Select the radius slider element
    const radiusSlider = document.getElementById("radiusSlider");
    // Set the initial value of the slider
    radiusSlider.value = "11"; // Set the default value to 11

    // Select the radius value span element
    const radiusValue = document.getElementById("radiusValue");

    // Update the text content of the radius value span element
    radiusValue.textContent = radiusSlider.value + " km";

    // Fetch the user's postcode from the server
    const response = await fetch('/Home/GetUserPostcode');
    const postcode = await response.text();

    // Check if the server returned an error
    if (response.ok) {
        // Pass the postcode to the initMap function
        initMap(postcode);
    } else {
        // Handle the error (e.g., show a message to the user)
        console.error('Failed to fetch the user\'s postcode:', postcode);
    }

    async function initMap(postcode) {
        // Use the provided postcode instead of "@Model.PostCode"
        defaultPostcode = postcode;

        // Geocode the postcode
        const geocoder = new google.maps.Geocoder();
        geocoder.geocode({ address: postcode }, function (results, status) {
            if (status === 'OK') {
                // Use the location of the first result as the center of the map
                const location = results[0].geometry.location;

                // Initialize the map with the user's coordinates
                map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 10,
                    center: location, // Center the map on the obtained coordinates
                    mapId: "DEMO_MAP_ID",
                    mapTypeControl: false,
                    mapTypeId: 'terrain',
                    fullscreenControl: false,
                    streetViewControl: false
                });

                // Add a radius circle centered around the user's location
                circle = new google.maps.Circle({
                    map: map,
                    center: location,
                    radius: 11000,
                    strokeColor: "#FFFFFF",
                    strokeOpacity: 1,
                    strokeWeight: 3,
                    fillColor: "#0F52BA",
                    fillOpacity: 0.2,
                });

                // Continue with other map initialization logic (e.g., slider, markers)
                initializeMapFeatures();

            } else {
                // Handle the error (e.g., show a message to the user)
                console.error('Geocode was not successful for the following reason: ' + status);
            }
        });
    }


    function initializeMapWithDefaultLocation() {
        // Initialize the map with default location (Glasgow)
        map = new google.maps.Map(document.getElementById("map"), {
            zoom: 10,
            center: { lat: 55.8642, lng: -4.2518 }, // Glasgow coordinates
            mapId: "DEMO_MAP_ID",
            mapTypeControl: false,
            mapTypeId: 'terrain',
            fullscreenControl: false,
            streetViewControl: false
        });

        // Add a radius circle centered around the default location (Glasgow)
        circle = new google.maps.Circle({
            map: map,
            center: { lat: 55.8642, lng: -4.2518 }, // Glasgow coordinates
            radius: 11000,
            strokeColor: "#FFFFFF",
            strokeOpacity: 1,
            strokeWeight: 3,
            fillColor: "#0F52BA",
            fillOpacity: 0.2,
        });

        // Continue with other map initialization logic (e.g., slider, markers)
        initializeMapFeatures();
    }

    function initializeMapFeatures() {
        // Listen for changes in the slider value and update the circle radius dynamically
        radiusSlider.addEventListener("input", function () {
            const newRadius = parseInt(radiusSlider.value);
            circle.setRadius(newRadius * 1000); // Convert kilometers to meters
            const radiusValue = newRadius.toFixed(1); // Round to 1 decimal place
            document.getElementById("radiusValue").textContent = radiusValue + " km";

            // Update markers visibility based on new circle radius
            updateMarkersVisibility(circle, markers);
        });

        // Add event listener to the "See Instructors" button
        const seeInstructorsBtn = document.getElementById("seeInstructorsBtn");
        seeInstructorsBtn.addEventListener("click", function () {
            // Call a function to fetch instructor data from the table and place markers on the map
            placeMarkersForInstructors();
        });

        // Add event listener to the "Find My Location" button
        const userLocationBtn = document.getElementById("userLocationBtn");
        userLocationBtn.addEventListener("click", function () {
            // Try to get the user's current position using Geolocation API
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    // Success callback
                    async function (position) {
                        // Get the user's current coordinates
                        const userLocation = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };

                        // Set the map center to the user's current location
                        map.setCenter(userLocation);
                        circle.setCenter(userLocation);
                        // Update markers visibility based on new circle radius
                        updateMarkersVisibility(circle, markers);

                        // Reverse geocode the user's current coordinates to get the postcode
                        const postcode = await getPostcodeFromCoordinates(userLocation);
                        if (postcode) {
                            // Update the radius value span with the user's postcode
                            document.getElementById("radiusValue").textContent = postcode;
                        } else {
                            console.error("Unable to retrieve postcode for user location.");
                        }
                    },
                    // Error callback
                    function (error) {
                        console.error("Error getting user location:", error);
                    }
                );
            } else {
                console.error("Geolocation is not supported by this browser.");
            }
        });
    }

    async function placeMarkersForInstructors() {
        try {
            // Select all table rows except the header row
            const rows = document.querySelectorAll('tbody tr');

            // Initialize an array to store instructor details
            const instructors = [];

            // Loop through each table row
            rows.forEach(row => {
                // Select the cells within the current row
                const cells = row.querySelectorAll('td');

                // Check if there are enough cells for instructor details
                if (cells.length >= 3) {
                    // Extract the instructor details from the cells
                    const name = cells[0].textContent.trim();
                    const postcode = cells[3].textContent.trim();
                    const rating = cells[4].textContent.trim();

                    // Log the instructor with their corresponding postcode to the console
                    console.log(`Instructor: ${name}, Postcode: ${postcode}, Rating: ${rating}`);

                    // Add the instructor details to the array
                    instructors.push({ name, postcode, rating });
                }
            });

            // Process the instructor data and place markers on the map
            createMarkersForInstructors(instructors);

        } catch (error) {
            console.error("Error placing markers for instructors:", error.message);
        }
    }

    function createMarkersForInstructors(instructors) {
        // Clear existing markers from the map and the markers array
        markers.forEach(marker => marker.setMap(null));
        markers = [];

        // Loop through the instructor data and create markers on the map
        instructors.forEach(function (instructor) {
            // Use geocoding to convert the postcode to geographic coordinates
            const geocoder = new google.maps.Geocoder();
            geocoder.geocode({ address: instructor.postcode }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK && results[0]) {
                    const location = results[0].geometry.location;

                    // Calculate the distance between the location and the circle's center
                    const distance = google.maps.geometry.spherical.computeDistanceBetween(location, circle.getCenter());

                    // Only create the marker if the location is within the circle's radius
                    if (distance <= circle.getRadius()) {
                        // Calculate the scaled size based on the current zoom level
                        const currentZoomLevel = map.getZoom();
                        const scaleFactor = currentZoomLevel / 10; // Adjust the scale factor as needed
                        const scaledSize = new google.maps.Size(16 * scaleFactor, 16 * scaleFactor);

                        // Create a marker for the instructor at the location
                        const marker = new google.maps.Marker({
                            position: location,
                            map: map,
                            title: `${instructor.name} - Rating: ${instructor.rating}/5`, // Set instructor name as marker title
                            animation: google.maps.Animation.DROP, // Add animation to marker
                            icon: {
                                url: '/img/mapcar.png', // Custom marker named "mapcar.png"
                                scaledSize: scaledSize // Set the scaled size of the custom marker icon
                            }
                        });

                        // Add event listener for marker click
                        marker.addListener('click', function () {
                            // Handle marker click event
                            // Get the corresponding table row
                            const rows = document.querySelectorAll('tbody tr');
                            rows.forEach(row => {
                                const cells = row.querySelectorAll('td');
                                if (cells.length >= 3) {
                                    const name = cells[0].textContent.trim();
                                    if (name === instructor.name) {
                                        // Change the background color of the row
                                        row.style.backgroundColor = 'palegreen';
                                    } else {
                                        row.style.backgroundColor = 'white';
                                    }
                                }
                            });
                        });

                        // Add the marker to the markers array
                        markers.push(marker);

                        // Log a message indicating that the marker was successfully placed
                        console.log("Marker placed for instructor:", instructor.name, instructor.rating);
                    }
                } else {
                    // Log a message indicating that the geocoding service couldn't find coordinates for the postcode
                    console.error("Geocode was not successful for postcode:", instructor.postcode, "Reason:", status);
                }
            });
        });
    }

    function updateMarkersVisibility(circle, markers) {
        // Update markers visibility based on whether they are inside or outside the circle
        markers.forEach(function (marker, index) {
            const distance = google.maps.geometry.spherical.computeDistanceBetween(marker.getPosition(), circle.getCenter());
            if (distance <= circle.getRadius()) {
                marker.setVisible(true);
                // Show corresponding table row
                const tableRow = document.querySelectorAll('tbody tr')[index];
                if (tableRow) {
                    tableRow.style.display = '';
                }
            } else {
                marker.setVisible(false);
                // Hide corresponding table row
                const tableRow = document.querySelectorAll('tbody tr')[index];
                if (tableRow) {
                    tableRow.style.display = 'none';
                }
            }
        });
    }

    // Function to get coordinates from postcode using Geocoding API
    async function getCoordinates(postcode) {
        return new Promise((resolve, reject) => {
            const geocoder = new google.maps.Geocoder();
            geocoder.geocode({ address: postcode }, (results, status) => {
                if (status === google.maps.GeocoderStatus.OK && results[0]) {
                    resolve(results[0].geometry.location);
                } else {
                    reject('Geocode was not successful for the following reason: ' + status);
                }
            });
        });
    }

    // Function to get postcode from coordinates using Geocoding API
    async function getPostcodeFromCoordinates(coordinates) {
        return new Promise((resolve, reject) => {
            const geocoder = new google.maps.Geocoder();
            geocoder.geocode({ location: coordinates }, (results, status) => {
                if (status === google.maps.GeocoderStatus.OK && results[0]) {
                    // Check if any address components contain postcode
                    const addressComponents = results[0].address_components;
                    addressComponents.forEach(component => {
                        if (component.types.includes('postal_code')) {
                            resolve(component.long_name);
                        }
                    });
                    // If no postcode found, reject with an error message
                    reject('No postcode found for the given coordinates.');
                } else {
                    reject('Geocode was not successful for the following reason: ' + status);
                }
            });
        });
    }

    // Call the initMap function to initialize the map
    initMap();
});

