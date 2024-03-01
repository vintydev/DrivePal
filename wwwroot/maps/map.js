// Wait for the DOM content to be fully loaded
document.addEventListener("DOMContentLoaded", function () {
    // Check if the page has been refreshed already
    if (!sessionStorage.getItem('pageRefreshed')) {
        // Set the flag to indicate that the page has been refreshed
        sessionStorage.setItem('pageRefreshed', 'true');
        // Refresh the page
        location.reload();
    }

    // Select the radius slider element
    const radiusSlider = document.getElementById("radiusSlider");
    // Set the initial value of the slider
    radiusSlider.value = "11"; // Set the default value to 11

    // Select the radius value span element
    const radiusValue = document.getElementById("radiusValue");

    // Update the text content of the radius value span element
    radiusValue.textContent = radiusSlider.value + " km";

    // Initialize and add the map
    let map;
    let circle;
    let markers = []; // Array to store markers
    let userMarker; // Variable to store the user's marker
    let postcodeMarker; // Variable to store the postcode marker

    async function initMap() {
        // Try to get the user's current position using Geolocation API
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                // Success callback
                function (position) {
                    // Get the user's current coordinates
                    const userLocation = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };

                    // Initialize the map with the user's current location as the center
                    map = new google.maps.Map(document.getElementById("map"), {
                        zoom: 10,
                        center: userLocation,
                        mapId: "DEMO_MAP_ID",
                        mapTypeControl: false,
                        mapTypeId: 'terrain',
                        fullscreenControl: false,
                        streetViewControl: false
                    });

                    // Add a marker at the user's location
                    userMarker = new google.maps.Marker({
                        position: userLocation,
                        map: map,
                        title: "Your Location",
                        animation: google.maps.Animation.DROP // Add animation to marker
                    });

                    // Add a radius circle centered around the user's location
                    circle = new google.maps.Circle({
                        map: map,
                        center: userLocation,
                        radius: 11000,
                        strokeColor: "#FFFFFF",
                        strokeOpacity: 1,
                        strokeWeight: 3,
                        fillColor: "#0F52BA",
                        fillOpacity: 0.2,
                    });

                    // Continue with other map initialization logic (e.g., slider, markers)
                    initializeMapFeatures();
                },
                // Error callback
                function (error) {
                    console.error("Error getting user location:", error);
                    // If getting user location fails, fallback to default location (London)
                    initializeMapWithDefaultLocation();
                }
            );
        } else {
            // If Geolocation API is not supported, fallback to default location (London)
            initializeMapWithDefaultLocation();
        }
    }

    function initializeMapWithDefaultLocation() {
        // Initialize the map with default location (London)
        map = new google.maps.Map(document.getElementById("map"), {
            zoom: 10,
            center: {
                lat: 51.5074,
                lng: 0.1278
            },
            mapId: "DEMO_MAP_ID",
            mapTypeControl: false,
            mapTypeId: 'terrain',
            fullscreenControl: false,
            streetViewControl: false
        });

        // Add a radius circle centered around the default location (London)
        circle = new google.maps.Circle({
            map: map,
            center: {
                lat: 51.5074,
                lng: 0.1278
            },
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
            // Get the center of the map
            const center = circle.getCenter();

            // Get the radius value from the slider
            const radius = parseInt(radiusSlider.value);

            // Remove markers outside the circle
            removeMarkersOutsideCircle(center, radius);

            // Generate and display a random marker inside the circle for each instructor
            generateRandomMarkersForInstructors(center, radius);
        });

        // Add event listener to the "Submit" button for postcode search
        const submitPostalCodeBtn = document.getElementById("submitPostalCode");
        submitPostalCodeBtn.addEventListener("click", function () {
            // Get the entered postal code
            const postalCode = document.getElementById("postalCodeInput").value;

            // Use geocoding to convert the entered postcode to geographic coordinates
            const geocoder = new google.maps.Geocoder();
            geocoder.geocode({
                address: postalCode
            }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    // Get the geographic coordinates (latitude and longitude) of the postcode
                    const location = results[0].geometry.location;

                    // Remove the previous postcode marker if it exists
                    if (postcodeMarker) {
                        postcodeMarker.setMap(null);
                    }

                    // Remove the user's marker if it exists
                    if (userMarker) {
                        userMarker.setMap(null);
                    }

                    // Set the map's center to the new coordinates
                    map.setCenter(location);

                    // Move circle to the new center
                    circle.setCenter(location);

                    // Add a marker at the location of the postcode search
                    postcodeMarker = new google.maps.Marker({
                        position: location,
                        map: map,
                        title: "Postcode Location",
                        animation: google.maps.Animation.DROP // Add animation to marker
                    });

                    // Update markers visibility based on new circle radius
                    updateMarkersVisibility(circle, markers);

                    // Remove markers from the previous search
                    removeMarkers();
                } else {
                    // Handle geocoding error
                    console.error("Geocode was not successful for the following reason: " + status);
                    alert("Geocode was not successful for the following reason: " + status);
                }
            });
        });
    }

    function generateRandomPosition(center, radius) {
        // Convert radius from kilometers to meters
        const metersRadius = radius * 1000;

        // Generate random distance and angle
        const randomDistance = Math.random() * metersRadius;
        const randomAngle = Math.random() * 2 * Math.PI;

        // Calculate random position
        const dx = randomDistance * Math.cos(randomAngle);
        const dy = randomDistance * Math.sin(randomAngle);
        const latLng = new google.maps.LatLng(
            center.lat() + (dy / 111111),
            center.lng() + (dx / (111111 * Math.cos(center.lat() * (Math.PI / 180))))
        );

        return latLng;
    }

    function generateRandomMarkersForInstructors(center, radius) {
        // Get all instructor names from the table
        const instructorNames = document.querySelectorAll(".instructor-name");

        // Generate and display a random marker inside the circle for each instructor
        instructorNames.forEach(function (instructor) {
            // Generate a random position inside the circle
            const position = generateRandomPosition(center, radius);

            // Create a new marker at the random position with custom icon
            const marker = new google.maps.Marker({
                map: map,
                position: position,
                title: instructor.textContent.trim(), // Set instructor name as marker title
                icon: {
                    url: '/img/mapcar.png', // Custom icon
                    scaledSize: new google.maps.Size(40, 40) // Set default size for the marker
                },
                animation: google.maps.Animation.DROP // Add animation to markers
            });

            // Add marker to the markers array
            markers.push(marker);

            // Log marker coordinates to console
            console.log("Marker Position:", marker.getPosition().lat(), marker.getPosition().lng());

            // Add event listener for marker click
            markers.forEach(function (marker) {
                marker.addListener('click', function () {
                    // Get the title of the clicked marker (instructor's name)
                    const instructorName = marker.getTitle();
                    console.log("Clicked Marker: " + instructorName); // Print the instructor's name to the console

                    // Find all table rows
                    const instructorRows = document.querySelectorAll('.instructor-row');

                    // Reset the background color of all table rows to white
                    instructorRows.forEach(row => {
                        row.style.backgroundColor = "white";
                    });

                    // Find the corresponding table row by instructor's name
                    const instructorRow = document.querySelector(`[data-instructor="${instructorName}"]`).closest('.instructor-row');

                    // Update the background color of the table row to pale green
                    instructorRow.style.backgroundColor = "#a6fca6";
                });
            });

        });
    }

    function removeMarkers() {
        // Remove all markers from the map
        markers.forEach(function (marker) {
            marker.setMap(null);
        });
    }

    function removeMarkersOutsideCircle(center, radius) {
        // Remove markers outside the circle
        markers.forEach(function (marker) {
            const distance = google.maps.geometry.spherical.computeDistanceBetween(marker.getPosition(), center);
            if (distance > radius) {
                marker.setMap(null);
            }
        });
    }

    function updateMarkersVisibility(circle, markers) {
        // Update markers visibility based on whether they are inside or outside the circle
        markers.forEach(function (marker) {
            const distance = google.maps.geometry.spherical.computeDistanceBetween(marker.getPosition(), circle.getCenter());
            if (distance <= circle.getRadius()) {
                marker.setVisible(true);
            } else {
                marker.setVisible(false);
            }
        });
    }

    // Call the initMap function to initialize the map
    initMap();
});
