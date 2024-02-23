document.addEventListener("DOMContentLoaded", function() {
    // Select the radius slider element
    const radiusSlider = document.getElementById("radiusSlider");
    // Set the initial value of the slider
    radiusSlider.value = "11"; // Set the default value to 10

    // Select the radius value span element
    const radiusValue = document.getElementById("radiusValue");

    // Update the text content of the radius value span element
    radiusValue.textContent = radiusSlider.value + " km";

    // Initialize and add the map
    let map;
    let marker;
    let circle;
    let position;

    async function initMap() {
        // The location of London
        position = { lat: 51.5074, lng: -0.1278 }; // London coordinates

        // Request needed libraries.
        //@ts-ignore
        const { Map } = await google.maps.importLibrary("maps");
        const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

        // The map, centered at London
        map = new Map(document.getElementById("map"), {
            zoom: 10,
            center: position,
            mapId: "DEMO_MAP_ID",
        });

        // The marker, positioned at London
        marker = new google.maps.marker.AdvancedMarkerElement({
            map: map,
            position: position,
            title: "London",
        });



        // Add a radius circle centered around the marker
        circle = new google.maps.Circle({
            map: map,
            center: position,
            radius: 11000, // Initial radius in meters
            strokeColor: "#FFFFFF",
            strokeOpacity: 1,
            strokeWeight: 3,
            fillColor: "#0F52BA",
            fillOpacity: 0.35,
        });

        // Listen for changes in the slider value and update the circle radius dynamically
        const radiusSlider = document.getElementById("radiusSlider");
        radiusSlider.addEventListener("input", function () {
            const newRadius = parseInt(radiusSlider.value);
            circle.setRadius(newRadius * 1000); // Convert kilometers to meters
            const radiusValue = (newRadius).toFixed(1); // Round to 1 decimal place
            document.getElementById("radiusValue").textContent = radiusValue + " km";
        });

        // Add event listener to the user location button
        const userLocationBtn = document.getElementById("userLocationBtn");
        userLocationBtn.addEventListener("click", getUserLocation);
    }

    function getUserLocation() {
        // Show loading spinner
        document.getElementById("loadingSpinner").style.display = "inline-block";
        console.log("getUserLocation function triggered"); // Debugging console log

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (pos) => {
                    const userPosition = {
                        lat: pos.coords.latitude,
                        lng: pos.coords.longitude
                    };
                    console.log("User position:", userPosition); // Debugging console log

                    // Update map center
                    map.setCenter(userPosition);

                    // Remove existing marker
                    if (marker) {
                        marker.setMap(null);
                    }

                    // Create a new marker at the user's location
                    marker = new google.maps.marker.AdvancedMarkerElement({
                        map: map,
                        position: userPosition,
                        title: "Your Location"
                    });


                    // Move circle to user's location
                    circle.setCenter(userPosition);

                    // Hide loading spinner
                    document.getElementById("loadingSpinner").style.display = "none";
                },
                (error) => {
                    console.error("Error getting user location:", error);
                    alert("Error getting user location. Please enable location services.");
                }
            );
        } else {
            alert("Geolocation is not supported by this browser.");
        }
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

    let marker1;
    let marker2;

function addRandomMarkers(center, radius) {
    console.log("Adding random markers...");

    // Generate two random positions inside the radius
    const position1 = generateRandomPosition(center, radius);
    const position2 = generateRandomPosition(center, radius);

    // Log the coordinates of the generated positions to the console
    console.log("Random Position 1:", position1.lat(), position1.lng());
    console.log("Random Position 2:", position2.lat(), position2.lng());

    // Create custom marker icon with the green dot
    const icon = {
        url: 'https://maps.google.com/mapfiles/ms/icons/green-dot.png',
        scaledSize: new google.maps.Size(32, 32),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(16, 32)
    };

    // Check if existing markers are outside the circle's radius and remove them
    if (marker1 && google.maps.geometry.spherical.computeDistanceBetween(marker1.getPosition(), center) > radius) {
        marker1.setMap(null);
        marker1 = null; // Reset marker variable
    }
    if (marker2 && google.maps.geometry.spherical.computeDistanceBetween(marker2.getPosition(), center) > radius) {
        marker2.setMap(null);
        marker2 = null; // Reset marker variable
    }

    // Create markers at the generated positions using AdvancedMarkerElement
    if (!marker1) {
        marker1 = new google.maps.marker.AdvancedMarkerElement({
            position: position1,
            map: map,
            title: "Instructor 1"
        });
        marker1.setIcon(icon1); // Set custom icon
    }

    if (!marker2) {
        marker2 = new google.maps.marker.AdvancedMarkerElement({
            position: position2,
            map: map,
            title: "Instructor 2"
        });
        marker2.setIcon(icon2); // Set custom icon
    }

    
    

    // Extend the bounds to include the new markers
    const bounds = new google.maps.LatLngBounds();
    if (marker1) {
        bounds.extend(marker1.getPosition());
    }
    if (marker2) {
        bounds.extend(marker2.getPosition());
    }
}   
    document.getElementById("seeInstructorsBtn").addEventListener("click", function() {
        // Get the center of the map
        const center = map.getCenter();

        // Get the radius value from the slider
        const radiusSlider = document.getElementById("radiusSlider");
        const radius = parseInt(radiusSlider.value);

        // Add random markers inside the radius
        addRandomMarkers(center, radius);
    });

    initMap();
});
