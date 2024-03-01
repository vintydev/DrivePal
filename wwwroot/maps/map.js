document.addEventListener("DOMContentLoaded", function () {
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
                    console.log("Map set to user location.");

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
            console.log("Error getting user location, defaulting to London.");
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
            // Call a function to fetch instructor data from your backend
            fetchInstructorsData();
        });
    }

    async function fetchInstructorsData() {
        try {
            // Make a request to your backend API to fetch instructor data
            const response = await fetch("/api/instructors");
            const data = await response.json();

            // Check if the response is successful and data is available
            if (response.ok && data) {
                // Process the instructor data and create markers on the map
                createMarkersForInstructors(data);
            } else {
                console.error("Error fetching instructor data:", response.statusText);
            }
        } catch (error) {
            console.error("Error fetching instructor data:", error.message);
        }
    }

    function createMarkersForInstructors(instructors) {
        // Loop through the instructor data and create markers on the map
        instructors.forEach(function (instructor) {
            // Extract instructor details (e.g., name, postcode) from the data
            const { firstName, lastName, postCode } = instructor;

            // Use geocoding to convert the postcode to geographic coordinates
            const geocoder = new google.maps.Geocoder();
            geocoder.geocode({ address: postCode }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK && results[0].geometry) {
                    const location = results[0].geometry.location;

                    // Log the instructor's details and coordinates to the console
                    console.log(`Instructor: ${firstName} ${lastName}, Postcode: ${postCode}, Coordinates: (${location.lat()}, ${location.lng()})`);

                    // Create a marker for the instructor at the location
                    const marker = new google.maps.Marker({
                        position: location,
                        map: map,
                        title: `${firstName} ${lastName}`, // Set instructor name as marker title
                        animation: google.maps.Animation.DROP // Add animation to marker
                    });

                    // Add event listener for marker click
                    marker.addListener('click', function () {
                        // Handle marker click event
                    });
                } else {
                    console.error("Geocode was not successful for the following reason:", status);
                }
            });
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
