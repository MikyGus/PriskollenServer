CREATE TABLE stores (
	id int auto_increment,
	name varchar(40) not null,
	image varchar(255),
	coordinate POINT NOT NULL,
	address varchar(40),
	city varchar(40),
	storechain_id int,
	created timestamp default current_timestamp,
	modified timestamp default current_timestamp,
	primary key(id),
	constraint fk_storechain
	foreign key(storechain_id)
		references storechains(id)
	);

INSERT INTO stores (name, image, coordinate, address, city, storechain_id) VALUES
('Ica Supermarket Hovshaga',null,Point(14.797456939889676,56.909225162189145),'Pomonavägen 6', 'Växjö',1),
('Ica Kvantum Norremark',null,Point(14.830311977284742,56.902149839127084),'Norremarksvägen 3', 'Växjö',1),
('Maxi Ica Stormarknad',null,Point(14.765980907577271,56.88369978800127),'Hejaregatan 30', 'Växjö',1);

DELIMITER $$
$$
CREATE PROCEDURE CreateStore(
	in Name varchar(40),
	in Image varchar(40),
	in Latitude double,
	in Longitude double,
	in address varchar(40),
	in city varchar(40),
	in storechain_id int)
BEGIN
	INSERT INTO stores (name, image, coordinate, address, city, storechain_id) 
	VALUES (Name, Image, Point(Longitude, Latitude), address, city, storechain_id)
	RETURNING id, name, image, 
		ST_AsText(coordinate), ST_X(coordinate) as longitude, ST_Y(coordinate) as latitude,
		address, city, storechain_id, created, modified ;
END$$

CREATE PROCEDURE GetAllStores()
BEGIN
	Select id, name, image, 
 		ST_Y(coordinate) as latitude, ST_X(coordinate) as longitude,
		address, city, storechain_id, created, modified 
	from stores
	order by name;
END$$

CREATE PROCEDURE GetAllStoresByDistance(in userlatitude double, in userlongitude double)
BEGIN
	Select id, name, image, 
		ST_X(coordinate) as longitude, ST_Y(coordinate) as latitude,
		ST_DISTANCE_SPHERE(coordinate,Point(userlongitude, userlatitude))/1000 as distance, 
	address, city, storechain_id, created, modified 
	from stores
	order by distance;
END$$

CREATE PROCEDURE UpdateStore(
	in SearchForId int,
	in Name varchar(40),
	in Image varchar(40),
	in Latitude double,
	in Longitude double,
	in Address varchar(40),
	in City varchar(40),
	in Storechain_id int)
BEGIN
	UPDATE stores
	SET name = Name, 
		image = Image,
		coordinate = Point(Longitude, Latitude),
		address = Address,
		city = City,
		storechain_id = Storechain_id,
		modified = CURRENT_TIMESTAMP()
	WHERE id = SearchForId;
	SELECT ROW_COUNT(); 
END$$

DELIMITER ;