<?php
$servername = "localhost";
$username = "yeraytm";
$password = "TeKhudcX6W";
$db = "yeraytm";

$conn = mysqli_connect($servername, $username, $password, $db);

if(!$conn)
{
    die("ERROR: Connection failed: " . mysqli_connect_error());
}

$type = $_POST['Type'];
$playerID = $_POST['PlayerID'];
$posX = $_POST['PositionX'];
$posY = $_POST['PositionY'];
$posZ = $_POST['PositionZ'];
$deltatime = $_POST['DeltaTime'];

$sql = "INSERT INTO SpatialTable (`Type`, `PlayerID`, `PositionX`, `PositionY`, `PositionZ`, `DeltaTime`)
        VALUES('$type', '$playerID', '$posX', '$posY', '$posZ', '$deltatime')";

if ($conn->query($sql) === TRUE)
{
    $eventID = $conn->insert_id;
    echo 'EventID: ' . $eventID;
}
?>