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

$sql = "SELECT * FROM SpatialTable ORDER BY EventID ASC";

$result = mysqli_query($conn, $sql);

if($result)
{
    while($row = mysqli_fetch_assoc($result))
    {
        echo $row["Type"] . "#" . 
        $row["PlayerID"] . "#" . 
        $row["PositionX"] . "#" . 
        $row["PositionY"] . "#" . 
        $row["PositionZ"] . "#" .
        $row["DeltaTime"] . 
        "*";
    }
}
else
{
    echo "QUERY ERROR";
}
?>