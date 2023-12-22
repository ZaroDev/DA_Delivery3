<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

// Database configuration
$serverName = "localhost";
$username = "victorfz";
$database = "victorfz";
$password = "gySRHFpY8YCK";

try {
    // Establish a connection to the database
    $conn = new mysqli($serverName, $username, $password, $database);

    // Check if a POST request with JSON data has been made
    if ($_SERVER['REQUEST_METHOD'] === 'GET') {

        // Replace "YourTableName" with the actual table name
        $sql = "SELECT * FROM player_paths";
        $result = $conn->query($sql);

        if ($result->num_rows > 0) {
            // Fetch all rows into an associative array
            $rows = array();
            while ($row = $result->fetch_assoc()) {
                $rows[] = $row;
            }

            // Convert the associative array to JSON
            $jsonData = json_encode($rows, JSON_PRETTY_PRINT);

            // Output the JSON data
            header('Content-Type: application/json');
            echo $jsonData;
        } else {
            echo "No data found.";
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