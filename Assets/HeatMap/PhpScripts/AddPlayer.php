<?php
// Database configuration
$serverName = "localhost";
$username = "victorfz";
$database = "victorfz";
$password = "gySRHFpY8YCK";

try {
    // Establish a connection to the database
    $conn = new mysqli($serverName, $username, $password, $database);

    // Check if a POST request with JSON data has been made
    if ($_SERVER['REQUEST_METHOD'] === 'POST') {
        // Retrieve the JSON data from the POST request
        $json_data = file_get_contents("php://input");

        // Decode the JSON data into a PHP associative array
        $data = json_decode($json_data, true);

        // Insert the data into the database
        $sql = "INSERT INTO `players` (`name`) VALUES (?, ?, ?)";
        if($stmt = $conn->prepare($sql)) {
            // Bind the values from the JSON data to the SQL parameters
            $stmt->bind_param('sss', $data['name']);
            // Execute the SQL query
            if($stmt->execute()) {
                // Creating the data
                $response = array(
                    'id' => $stmt->insert_id
                );
                // Send the ID back to Unity
                echo json_encode($response);
            } else {
                echo "Error: " . $conn->error;
            }

            $stmt->close();
        }
        
    } else {
        // Handle the case where no JSON data was received
        echo "No JSON data received.";
    }
    $conn->close();
} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}
?>