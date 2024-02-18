CREATE TABLE storechains (
	id int auto_increment,
	name varchar(50) not null,
	image varchar(255),
	created timestamp default current_timestamp,
	modified timestamp default current_timestamp,

	primary key(id)
);

INSERT INTO storechains (name, image) VALUES
('Ica', null),
('City Gross', null),
('Coop', null);