CREATE TABLE store (
	id int auto_increment,
	name varchar(40) not null,
	image varchar(255),
	coordinate POINT NOT NULL,
	address varchar(40),
	city varchar(40),
	storechain_id int,
	created timestamp default current_timestamp,
	modified timestamp default current_timestamp,
	primary key(id)
	);

INSERT INTO store (name, image, coordinate, address, city, storechain_id) VALUES
('Ica Supermarket Hovshaga',null,Point(14.797456939889676,56.909225162189145),'Pomonavägen 6', 'Växjö',1),
('Ica Kvantum Norremark',null,Point(14.830311977284742,56.902149839127084),'Norremarksvägen 3', 'Växjö',1),
('Maxi Ica Stormarknad',null,Point(14.765980907577271,56.88369978800127),'Hejaregatan 30', 'Växjö',1);

DELIMITER $$
$$
CREATE PROCEDURE GetAllStore()
BEGIN
	Select id, name, image, ST_AsText(coordinate), address, city, storechain_id, created, modified 
	from store
	order by name;
END$$
CREATE PROCEDURE GetAllStoreByDistance(in userlatitude double, in userlongitude double)
BEGIN
	Select id, name, image, ST_AsText(coordinate) as coordinate, 
	ST_DISTANCE_SPHERE(coordinate,Point(userlongitude, userlatitude))/1000 as distance, 
	address, city, storechain_id, created, modified 
	from store
	order by distance;
END$$
DELIMITER ;