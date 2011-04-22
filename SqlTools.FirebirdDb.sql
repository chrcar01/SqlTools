
recreate table State (
    Code char(2) not null,
    Abbreviation char(2) not null,
    Name varchar(30) not null,
    Display varchar(30) not null,
    LastUpdated date,
    constraint pk_State primary key(Code)
);

commit;

INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('01', 'AL', 'Alabama', 'Alabama');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('02', 'AK', 'Alaska', 'Alaska');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('04', 'AZ', 'Arizona', 'Arizona');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('05', 'AR', 'Arkansas', 'Arkansas');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('06', 'CA', 'California', 'California');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('08', 'CO', 'Colorado', 'Colorado');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('09', 'CT', 'Connecticut', 'Connecticut');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('10', 'DE', 'Delaware', 'Delaware');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('11', 'DC', 'District of Columbia', 'District of Columbia');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('12', 'FL', 'Florida', 'Florida');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('13', 'GA', 'Georgia', 'Georgia');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('15', 'HI', 'Hawaii', 'Hawaii');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('16', 'ID', 'Idaho', 'Idaho');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('17', 'IL', 'Illinois', 'Illinois');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('18', 'I', 'Indiana', 'Indiana');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('19', 'IA', 'Iowa', 'Iowa');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('20', 'KS', 'Kansas', 'Kansas');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('21', 'KY', 'Kentucky', 'Kentucky');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('22', 'LA', 'Louisiana', 'Louisiana');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('23', 'ME', 'Maine', 'Maine');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('24', 'MD', 'Maryland', 'Maryland');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('25', 'MA', 'Massachusetts', 'Massachusetts');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('26', 'MI', 'Michiga', 'Michiga');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('27', 'M', 'Minnesota', 'Minnesota');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('28', 'MS', 'Mississippi', 'Mississippi');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('29', 'MO', 'Missouri', 'Missouri');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('30', 'MT', 'Montana', 'Montana');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('31', 'NE', 'Nebraska', 'Nebraska');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('32', 'NV', 'Nevada', 'Nevada');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('33', 'NH', 'New Hampshire', 'New Hampshire');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('34', 'NJ', 'New Jersey', 'New Jersey');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('35', 'NM', 'New Mexico', 'New Mexico');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('36', 'NY', 'New York', 'New York');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('37', 'NC', 'North Carolina', 'North Carolina');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('38', 'ND', 'North Dakota', 'North Dakota');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('39', 'OH', 'Ohio', 'Ohio');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('40', 'OK', 'Oklahoma', 'Oklahoma');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('41', 'OR', 'Orego', 'Orego');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('42', 'PA', 'Pennsylvania', 'Pennsylvania');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('44', 'RI', 'Rhode Island', 'Rhode Island');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('45', 'SC', 'South Carolina', 'South Carolina');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('46', 'SD', 'South Dakota', 'South Dakota');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('47', 'T', 'Tennessee', 'Tennessee');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('48', 'TX', 'Texas', 'Texas');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('49', 'UT', 'Utah', 'Utah');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('50', 'VT', 'Vermont', 'Vermont');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('51', 'VA', 'Virginia', 'Virginia');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('53', 'WA', 'Washingto', 'Washingto');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('54', 'WV', 'West Virginia', 'West Virginia');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('55', 'WI', 'Wisconsi', 'Wisconsi');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('56', 'WY', 'Wyoming', 'Wyoming');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('60', 'AS', 'American Samoa', 'American Samoa');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('64', 'FM', 'Federated States of Micronesia', 'Micronesia');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('66', 'GU', 'Guam', 'Guam');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('68', 'MH', 'Marshall Islands', 'Marshall Islands');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('69', 'MP', 'Northern Mariana Islands', 'Mariana Islands');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('70', 'PW', 'Palau', 'Palau');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('72', 'PR', 'Puerto Rico', 'Puerto Rico');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('74', 'UM', 'U.S. Minor Outlying Islands', 'Outlying Islands');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('78', 'VI', 'Virgin Islands of the U.S.', 'Virgin Islands');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('79', 'AB', 'Alberta', 'Alberta');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('80', 'BC', 'British Columbia', 'British Columbia');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('81', 'MB', 'Manitoba', 'Manitoba');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('82', 'NL', 'Newfoundland and Labrador', 'Newfoundland');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('83', 'NT', 'Northwest Territories', 'Northwest Territories');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('84', 'NS', 'Nunavut', 'Nunavut');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('85', 'O', 'Ontario', 'Ontario');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('86', 'PE', 'Prince Edward Island', 'Prince Edward Island');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('87', 'QC', 'Quebec', 'Quebec');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('88', 'SK', 'Saskatchewa', 'Saskatchewa');
INSERT INTO State (Code, Abbreviation, Name, Display) VALUES ('89', 'YT', 'Yuko', 'Yuko');

commit;

recreate table Customer(
	ID int not null,
	FirstName varchar(30) not null,
	MiddleInitial char(1),
	LastName varchar(30) not null,
	constraint pk_Customer primary key(ID)
);
commit;

insert into Customer(ID, FirstName, LastName, MiddleInitial)values(1, 'Chris', 'Carter', 'J')
insert into Customer(ID, FirstName, LastName)values(2, 'Jimmy', 'Smith')
commit;
