/* =====================================================================
   HOTEL MANAGEMENT SYSTEM - FINAL COMBINED SQL SERVER SCRIPT
   =====================================================================

   This script combines:
   1. Original Database + Tables + Sample Data
   2. Authentication & Role Management
   3. Reservation/User Relationship
   4. Hotel-Room Relationship
   5. Room & Reservation Status
   6. Audit Columns
   7. Soft Delete Support
   8. Sample Admin/User Accounts

   ===================================================================== */


/* =====================================================================
   PART 1 : ORIGINAL DATABASE STRUCTURE
   ===================================================================== */

/* ======================================================
   Drop & Create Database
   ====================================================== */
IF DB_ID('hotel') IS NOT NULL
BEGIN
    DROP DATABASE hotel;
END;
GO

CREATE DATABASE hotel;
GO

USE hotel;
GO

/* ======================================================
   Hotel
   ====================================================== */
IF OBJECT_ID('Hotel', 'U') IS NULL
BEGIN
    CREATE TABLE Hotel (
        hotel_id INT IDENTITY(1,1) NOT NULL,
        name VARCHAR(255),
        location VARCHAR(255),
        description VARCHAR(MAX),
        CONSTRAINT pk_Hotel PRIMARY KEY (hotel_id)
    );
END;
GO

/* ======================================================
   RoomType
   ====================================================== */
IF OBJECT_ID('RoomType', 'U') IS NULL
BEGIN
    CREATE TABLE RoomType (
        room_type_id INT IDENTITY(1,1) NOT NULL,
        type_name VARCHAR(255),
        description VARCHAR(MAX),
        max_occupancy INT,
        price_per_night DECIMAL(10,2),
        CONSTRAINT pk_RoomType PRIMARY KEY (room_type_id)
    );
END;
GO

/* ======================================================
   Room
   ====================================================== */
IF OBJECT_ID('Room', 'U') IS NULL
BEGIN
    CREATE TABLE Room (
        room_id INT IDENTITY(1,1) NOT NULL,
        room_number INT,
        room_type_id INT,
        is_available BIT,
        CONSTRAINT pk_Room PRIMARY KEY (room_id),
        CONSTRAINT fk_Room_RoomType
            FOREIGN KEY (room_type_id)
            REFERENCES RoomType(room_type_id)
    );
END;
GO

/* ======================================================
   Reservation
   ====================================================== */
IF OBJECT_ID('Reservation', 'U') IS NULL
BEGIN
    CREATE TABLE Reservation (
        reservation_id INT IDENTITY(1,1) NOT NULL,
        guest_name VARCHAR(255),
        guest_email VARCHAR(255),
        guest_phone VARCHAR(20),
        check_in_date DATE,
        check_out_date DATE,
        room_id INT,
        CONSTRAINT pk_Reservation PRIMARY KEY (reservation_id),
        CONSTRAINT fk_Reservation_Room
            FOREIGN KEY (room_id)
            REFERENCES Room(room_id)
    );
END;
GO

/* ======================================================
   Amenity
   ====================================================== */
IF OBJECT_ID('Amenity', 'U') IS NULL
BEGIN
    CREATE TABLE Amenity (
        amenity_id INT IDENTITY(1,1) NOT NULL,
        name VARCHAR(255),
        description VARCHAR(MAX),
        CONSTRAINT pk_Amenity PRIMARY KEY (amenity_id)
    );
END;
GO

/* ======================================================
   HotelAmenity (M:N)
   ====================================================== */
IF OBJECT_ID('HotelAmenity', 'U') IS NULL
BEGIN
    CREATE TABLE HotelAmenity (
        hotel_id INT NOT NULL,
        amenity_id INT NOT NULL,
        CONSTRAINT pk_HotelAmenity PRIMARY KEY (hotel_id, amenity_id),
        CONSTRAINT fk_HotelAmenity_Hotel
            FOREIGN KEY (hotel_id)
            REFERENCES Hotel(hotel_id),
        CONSTRAINT fk_HotelAmenity_Amenity
            FOREIGN KEY (amenity_id)
            REFERENCES Amenity(amenity_id)
    );
END;
GO

/* ======================================================
   RoomAmenity (M:N)
   ====================================================== */
IF OBJECT_ID('RoomAmenity', 'U') IS NULL
BEGIN
    CREATE TABLE RoomAmenity (
        room_id INT NOT NULL,
        amenity_id INT NOT NULL,
        CONSTRAINT pk_RoomAmenity PRIMARY KEY (room_id, amenity_id),
        CONSTRAINT fk_RoomAmenity_Room
            FOREIGN KEY (room_id)
            REFERENCES Room(room_id),
        CONSTRAINT fk_RoomAmenity_Amenity
            FOREIGN KEY (amenity_id)
            REFERENCES Amenity(amenity_id)
    );
END;
GO

/* ======================================================
   Payment
   ====================================================== */
IF OBJECT_ID('Payment', 'U') IS NULL
BEGIN
    CREATE TABLE Payment (
        payment_id INT IDENTITY(1,1) NOT NULL,
        reservation_id INT,
        amount DECIMAL(10,2),
        payment_date DATE,
        payment_status VARCHAR(50),
        CONSTRAINT pk_Payment PRIMARY KEY (payment_id),
        CONSTRAINT fk_Payment_Reservation
            FOREIGN KEY (reservation_id)
            REFERENCES Reservation(reservation_id)
    );
END;
GO

/* ======================================================
   Review
   ====================================================== */
IF OBJECT_ID('Review', 'U') IS NULL
BEGIN
    CREATE TABLE Review (
        review_id INT IDENTITY(1,1) NOT NULL,
        reservation_id INT,
        rating INT,
        comment VARCHAR(MAX),
        review_date DATE,
        CONSTRAINT pk_Review PRIMARY KEY (review_id),
        CONSTRAINT fk_Review_Reservation
            FOREIGN KEY (reservation_id)
            REFERENCES Reservation(reservation_id)
    );
END;
GO

--Insert Hotel


INSERT INTO Hotel (name, location, description) VALUES
('Grand Plaza Hotel', 'Downtown City Center', 'Luxury hotel with stunning views of the city.'),
('Oceanfront Resort &amp; Spa', 'Beachfront Paradise', 'Relaxing resort with spa facilities, steps away from the ocean.'),
('Downtown Oasis Hotel', 'City Center', 'Modern hotel with a central location, perfect for business travelers.'),
('Seaside Retreat Lodge', 'Coastal Area', 'Scenic lodge offering a peaceful escape by the sea.'),
('Mountain View Hotel', 'Mountainous Region', 'Elegant hotel surrounded by breathtaking mountain views.'),
('Urban Skyline Suites', 'Metropolitan Area', 'Chic suites with panoramic views of the city skyline.'),
('Serenity Valley Resort', 'Nature Retreat', 'Tranquil resort nestled in a serene valley for nature enthusiasts.'),
('Historic Heritage Inn', 'Historical District', 'Charming inn showcasing rich cultural heritage.'),
('Snowy Peaks Chalet', 'Mountain Resort', 'Cozy chalet with a fireplace, perfect for winter getaways.'),
('Riverside Boutique Hotel', 'Riverside District', 'Boutique hotel offering a blend of comfort and style by the river.'),
('Grand Plaza Hotel', 'Downtown City Center', 'Luxury hotel with stunning views of the city.'),
('Oceanfront Resort &amp; Spa', 'Beachfront Paradise', 'Relaxing resort with spa facilities, steps away from the ocean.'),
('Downtown Oasis Hotel', 'City Center', 'Modern hotel with a central location, perfect for business travelers.'),
('Seaside Retreat Lodge', 'Coastal Area', 'Scenic lodge offering a peaceful escape by the sea.'),
('Mountain View Hotel', 'Mountainous Region', 'Elegant hotel surrounded by breathtaking mountain views.'),
('Urban Skyline Suites', 'Metropolitan Area', 'Chic suites with panoramic views of the city skyline.'),
('Serenity Valley Resort', 'Nature Retreat', 'Tranquil resort nestled in a serene valley for nature enthusiasts.'),
('Historic Heritage Inn', 'Historical District', 'Charming inn showcasing rich cultural heritage.'),
('Snowy Peaks Chalet', 'Mountain Resort', 'Cozy chalet with a fireplace, perfect for winter getaways.'),
('Riverside Boutique Hotel', 'Riverside District', 'Boutique hotel offering a blend of comfort and style by the river.'),
('Grand Plaza Hotel', 'Downtown City Center', 'Luxury hotel with stunning views of the city.'),
('Oceanfront Resort &amp; Spa', 'Beachfront Paradise', 'Relaxing resort with spa facilities, steps away from the ocean.'),
('Downtown Oasis Hotel', 'City Center', 'Modern hotel with a central location, perfect for business travelers.'),
('Seaside Retreat Lodge', 'Coastal Area', 'Scenic lodge offering a peaceful escape by the sea.'),
('Mountain View Hotel', 'Mountainous Region', 'Elegant hotel surrounded by breathtaking mountain views.'),
('Urban Skyline Suites', 'Metropolitan Area', 'Chic suites with panoramic views of the city skyline.'),
('Serenity Valley Resort', 'Nature Retreat', 'Tranquil resort nestled in a serene valley for nature enthusiasts.'),
('Historic Heritage Inn', 'Historical District', 'Charming inn showcasing rich cultural heritage.'),
('Snowy Peaks Chalet', 'Mountain Resort', 'Cozy chalet with a fireplace, perfect for winter getaways.'),
('Riverside Boutique Hotel', 'Riverside District', 'Boutique hotel offering a blend of comfort and style by the river.'),
('City Lights Hotel', 'Downtown Core', 'Captivating city lights view from every room.'),
('Island Paradise Resort', 'Tropical Island', 'Escape to a paradise resort surrounded by pristine beaches.'),
('Alpine Retreat Lodge', 'Alpine Meadows', 'Inviting lodge surrounded by alpine meadows and hiking trails.'),
('Business Traveler Inn', 'Business District', 'Tailored for the needs of business travelers.'),
('Golden Sands Resort', 'Desert Oasis', 'Luxurious resort oasis in the middle of the desert.'),
('Harbor View Hotel', 'Harborfront District', 'Enjoy scenic harbor views in this waterfront hotel.'),
('Sky High Tower Suites', 'Skyscraper District', 'Exclusive suites located in the tallest tower in the city.'),
('Hidden Valley Resort', 'Hidden Valley', 'Discover the hidden gem of a resort in a secluded valley.'),
('Colonial Heritage Inn', 'Colonial District', 'Step back in time at this colonial-style heritage inn.'),
('Mystic Forest Lodge', 'Enchanted Forest', 'Immerse yourself in the magic of a lodge nestled in a mystical forest.'),
('Sunny Meadows Hotel', 'Meadowlands', 'Bright and sunny hotel surrounded by picturesque meadows.'),
('Lakeside Serenity Resort', 'Lakeside Retreat', 'Find serenity by the lakeside in this peaceful resort.'),
('Penthouse Sky Suites', 'Luxury Skyline', 'Indulge in luxury with penthouse suites boasting skyline views.'),
('Palm Grove Resort', 'Tropical Haven', 'Relax under the palm trees in this tropical haven.'),
('Whispering Pines Inn', 'Pine Forest', 'Cozy inn surrounded by the whispering sounds of pine trees.'),
('Sapphire Shores Hotel', 'Shorefront Paradise', 'Experience luxury on the shores of a sapphire-blue ocean.'),
('Countryside Escape Lodge', 'Countryside', 'Escape to the countryside in this charming and rustic lodge.'),
('Cultural Heritage Hotel', 'Cultural District', 'Immerse yourself in the rich cultural heritage of this hotel.'),
('Coastal Breeze Resort', 'Coastal Getaway', 'Feel the coastal breeze in this relaxing and picturesque resort.');

--Room Type

INSERT INTO RoomType (type_name, description, max_occupancy, price_per_night) VALUES
('Single', 'Single bed room', 1, 50.00),
('Double', 'Double bed room', 2, 80.00),
('Suite', 'Luxury suite', 4, 150.00),
('Penthouse', 'Exclusive penthouse', 6, 300.00),
('Deluxe Single', 'Deluxe single bed room', 1, 70.00),
('Ocean View Double', 'Double bed room with ocean view', 2, 100.00),
('Executive Suite', 'Executive suite with city views', 4, 200.00),
('Royal Penthouse', 'Royal penthouse with panoramic skyline views', 6, 400.00),
('Business Traveler Room', 'Tailored for the needs of business travelers', 1, 90.00),
('Mountain Retreat Double', 'Double bed room with mountain views', 2, 120.00),
('Presidential Suite', 'Presidential suite with opulent amenities', 6, 500.00),
('Sky High Penthouse', 'Penthouse in the tallest skyscraper', 8, 600.00),
('Tropical Paradise Single', 'Single bed room in a tropical paradise', 1, 80.00),
('Lakeside Double', 'Double bed room with lakeside views', 2, 110.00),
('Family Suite', 'Spacious suite for the whole family', 6, 250.00),
('Luxury Spa Penthouse', 'Penthouse with exclusive spa facilities', 8, 700.00),
('Business Class Single', 'Business class single room with workspace', 1, 100.00),
('Forest View Double', 'Double bed room with forest views', 2, 130.00),
('Grand Presidential Suite', 'Grand presidential suite with lavish decor', 8, 800.00),
('Urban Escape Penthouse', 'Penthouse offering an urban escape', 6, 450.00),
('Sunny Meadows Single', 'Single bed room with views of sunny meadows', 1, 90.00),
('Lakefront Double', 'Double bed room with lakefront views', 2, 120.00),
('Family Vacation Suite', 'Suite perfect for a family vacation', 6, 280.00),
('Skyline Luxury Penthouse', 'Luxury penthouse with stunning skyline views', 8, 750.00),
('Seaside Retreat Single', 'Single bed room in a seaside retreat', 1, 100.00),
('Beachfront Double', 'Double bed room with direct beach access', 2, 130.00),
('Honeymoon Suite', 'Romantic suite ideal for honeymooners', 2, 180.00),
('City Lights Penthouse', 'Penthouse with breathtaking city lights', 6, 500.00),
('Mountain Serenity Single', 'Single bed room offering mountain serenity', 1, 80.00),
('River View Double', 'Double bed room with picturesque river views', 2, 110.00),
('Executive Family Suite', 'Executive suite designed for families', 6, 260.00),
('Luxury Skyline Penthouse', 'Luxury penthouse with panoramic skyline views', 8, 700.00),
('Standard Single', 'Basic single bed room', 1, 55.00),
('Double with a View', 'Double bed room with scenic views', 2, 90.00),
('Executive Suite', 'Spacious executive suite', 4, 180.00),
('Luxury Penthouse', 'Luxurious penthouse with premium amenities', 6, 350.00),
('Premium Single', 'Premium single bed room with extra amenities', 1, 75.00),
('Seaview Double', 'Double bed room with stunning sea views', 2, 110.00),
('Cityscape Suite', 'Suite with breathtaking cityscape views', 4, 220.00),
('Skyline Penthouse', 'Penthouse with panoramic skyline views', 6, 400.00),
('Business Class Single', 'Business class single room with workspace', 1, 95.00),
('Mountain Retreat Double', 'Double bed room with mountain retreat vibes', 2, 125.00),
('Presidential Suite', 'Presidential suite with opulent furnishings', 6, 520.00),
('Sky High Penthouse', 'Penthouse on the highest floor', 8, 620.00),
('Tropical Escape Single', 'Single bed room in a tropical escape', 1, 85.00),
('Lakeview Double', 'Double bed room with serene lake views', 2, 115.00),
('Family Oasis Suite', 'Suite designed for a family oasis', 6, 260.00),
('Spa Retreat Penthouse', 'Penthouse with private spa retreat', 8, 720.00),
('Business Traveler Single', 'Ideal room for business travelers', 1, 105.00),
('Forest View Double', 'Double bed room with forest views', 2, 135.00),
('Grand Presidential Suite', 'Grand presidential suite with grandeur', 8, 830.00),
('Urban Oasis Penthouse', 'Penthouse offering an urban oasis', 6, 480.00),
('Sunset Meadows Single', 'Single bed room with views of sunset meadows', 1, 95.00),
('Lakeside Luxury Double', 'Luxury double bed room with lakeside views', 2, 125.00),
('Family Adventure Suite', 'Adventure-themed suite for the family', 6, 270.00),
('Panoramic Skyline Penthouse', 'Penthouse with panoramic skyline views', 8, 770.00),
('Coastal Serenity Single', 'Single bed room with coastal serenity', 1, 105.00),
('Beachfront Bliss Double', 'Double bed room with direct beachfront access', 2, 135.00),
('Romantic Escape Suite', 'Suite designed for a romantic escape', 2, 185.00),
('City Lights Penthouse', 'Penthouse with mesmerizing city lights', 6, 520.00),
('Mountain Tranquility Single', 'Single bed room offering mountain tranquility', 1, 85.00),
('Riverside Double', 'Double bed room with views of the picturesque river', 2, 115.00),
('Executive Family Suite', 'Executive suite perfect for families', 6, 250.00),
('Luxury Skyline Penthouse', 'Luxury penthouse with sweeping skyline views', 8, 690.00);

--insert ROOM
INSERT INTO Room (room_number, room_type_id, is_available) VALUES
(101, 1, 1),
(102, 2, 0),
(201, 3, 1),
(301, 4, 0),
(103, 1, 1),
(104, 2, 1),
(105, 3, 0),
(106, 4, 1),
(107, 1, 0),
(108, 2, 1),
(109, 3, 1),
(110, 4, 0),
(201, 1, 1),
(202, 2, 0),
(203, 3, 1),
(204, 4, 0),
(205, 1, 1),
(206, 2, 1),
(207, 3, 0),
(208, 4, 1),
(209, 1, 0),
(210, 2, 1),
(301, 3, 1),
(302, 4, 0),
(303, 1, 1),
(304, 2, 0),
(305, 3, 1),
(306, 4, 1),
(307, 1, 0),
(308, 2, 1),
(309, 3, 1),
(310, 4, 0),
(401, 1, 1),
(402, 2, 0),
(403, 3, 1),
(404, 4, 0),
(405, 1, 1),
(406, 2, 1),
(407, 3, 0),
(408, 4, 1),
(409, 1, 0),
(410, 2, 1),
(501, 3, 1),
(502, 4, 0),
(503, 1, 1),
(504, 2, 0),
(505, 3, 1),
(506, 4, 1),
(507, 1, 0),
(508, 2, 1),
(509, 3, 1),
(510, 4, 0);

-- Insert records into the Reservation table
INSERT INTO Reservation
(guest_name, guest_email, guest_phone, check_in_date, check_out_date, room_id)
VALUES
('John Doe', 'john@example.com', '1234567890', '2024-01-01', '2024-01-05', 1),
('Jane Smith', 'jane@example.com', '9876543210', '2024-02-01', '2024-02-05', 2),
('Mike Johnson', 'mike@example.com', '5551234567', '2024-03-01', '2024-03-05', 3),
('Alice Lee', 'alice@example.com', '1112223333', '2024-04-01', '2024-04-05', 4),
('Ethan Brown', 'ethan@example.com', '7778889999', '2024-05-01', '2024-05-05', 5),
('Olivia Wilson', 'olivia@example.com', '2223334444', '2024-06-01', '2024-06-05', 6),
('Liam Davis', 'liam@example.com', '9990001111', '2024-07-01', '2024-07-05', 7),
('Ava Miller', 'ava@example.com', '4445556666', '2024-08-01', '2024-08-05', 8),
('Noah Martinez', 'noah@example.com', '1234567890', '2024-09-01', '2024-09-05', 9),
('Sophia Jones', 'sophia@example.com', '9876543210', '2024-10-01', '2024-10-05', 10),
('Jackson Taylor', 'jackson@example.com', '5551234567', '2024-11-01', '2024-11-05', 11),
('Emma White', 'emma@example.com', '1112223333', '2024-12-01', '2024-12-05', 12),
('Aiden Harris', 'aiden@example.com', '7778889999', '2025-01-01', '2025-01-05', 13),
('Mia Garcia', 'mia@example.com', '2223334444', '2025-02-01', '2025-02-05', 14),
('Lucas Rodriguez', 'lucas@example.com', '9990001111', '2025-03-01', '2025-03-05', 15),
('Aria Martinez', 'aria@example.com', '4445556666', '2025-04-01', '2025-04-05', 16),
('Liam Davis', 'liam@example.com', '1234567890', '2025-05-01', '2025-05-05', 17),
('Sophia Smith', 'sophia@example.com', '9876543210', '2025-06-01', '2025-06-05', 18),
('Logan Taylor', 'logan@example.com', '5551234567', '2025-07-01', '2025-07-05', 19),
('Olivia Johnson', 'olivia@example.com', '1112223333', '2025-08-01', '2025-08-05', 20),
('Ethan Brown', 'ethan@example.com', '7778889999', '2025-09-01', '2025-09-05', 21),
('Ava Wilson', 'ava@example.com', '2223334444', '2025-10-01', '2025-10-05', 22),
('Jackson Davis', 'jackson@example.com', '9990001111', '2025-11-01', '2025-11-05', 23),
('Emma Miller', 'emma@example.com', '4445556666', '2025-12-01', '2025-12-05', 24),
('Noah Harris', 'noah@example.com', '1234567890', '2026-01-01', '2026-01-05', 25),
('Mia Jones', 'mia@example.com', '9876543210', '2026-02-01', '2026-02-05', 26),
('Lucas Taylor', 'lucas@example.com', '5551234567', '2026-03-01', '2026-03-05', 27),
('Aria White', 'aria@example.com', '1112223333', '2026-04-01', '2026-04-05', 28),
('Aiden Johnson', 'aiden@example.com', '7778889999', '2026-05-01', '2026-05-05', 29),
('Sophia Garcia', 'sophia@example.com', '2223334444', '2026-06-01', '2026-06-05', 30),
('Logan Rodriguez', 'logan@example.com', '9990001111', '2026-07-01', '2026-07-05', 31),
('Olivia Martinez', 'olivia@example.com', '4445556666', '2026-08-01', '2026-08-05', 32),
('Ethan Davis', 'ethan@example.com', '1234567890', '2026-09-01', '2026-09-05', 33),
('Ava Smith', 'ava@example.com', '9876543210', '2026-10-01', '2026-10-05', 34),
('Jackson Taylor', 'jackson@example.com', '5551234567', '2026-11-01', '2026-11-05', 35),
('Emma Johnson', 'emma@example.com', '1112223333', '2026-12-01', '2026-12-05', 36),
('Noah Miller', 'noah@example.com', '7778889999', '2027-01-01', '2027-01-05', 37),
('Mia Harris', 'mia@example.com', '2223334444', '2027-02-01', '2027-02-05', 38),
('Lucas Garcia', 'lucas@example.com', '9990001111', '2027-03-01', '2027-03-05', 39),
('Aria Martinez', 'aria@example.com', '4445556666', '2027-04-01', '2027-04-05', 40),
('Liam Davis', 'liam@example.com', '1234567890', '2027-05-01', '2027-05-05', 41),
('Sophia Smith', 'sophia@example.com', '9876543210', '2027-06-01', '2027-06-05', 42),
('Logan Taylor', 'logan@example.com', '5551234567', '2027-07-01', '2027-07-05', 43),
('Olivia Johnson', 'olivia@example.com', '1112223333', '2027-08-01', '2027-08-05', 44),
('Ethan Brown', 'ethan@example.com', '7778889999', '2027-09-01', '2027-09-05', 45),
('Sophie Anderson', 'sophie@example.com', '5556667777', '2027-11-01', '2027-11-05', 46),
('Carter Wilson', 'carter@example.com', '8889990000', '2027-12-01', '2027-12-05', 47),
('Madison Davis', 'madison@example.com', '2223334444', '2028-01-01', '2028-01-05', 48),
('Elijah White', 'elijah@example.com', '9990001111', '2028-02-01', '2028-02-05', 49),
('Grace Taylor', 'grace@example.com', '3334445555', '2028-03-01', '2028-03-05', 50);


-- Insert records into the Amenity table
INSERT INTO Amenity (name, description) VALUES
('Wi-Fi', 'High-speed internet access'),
('Pool', 'Swimming pool with lounge area'),
('Gym', 'Fully equipped fitness center'),
('Spa', 'Relaxing spa and wellness services'),
('Parking', 'Secure parking facilities'),
('Restaurant', 'On-site restaurant serving a variety of cuisines'),
('Conference Room', 'Meeting and conference room facilities'),
('Shuttle Service', 'Complimentary shuttle service for guests'),
('Pet-Friendly', 'Pet-friendly accommodations'),
('24-Hour Reception', 'Round-the-clock reception desk services'),
('Laundry Service', 'In-house laundry and dry-cleaning services'),
('Business Center', 'Dedicated business center with office amenities'),
('Childcare Services', 'Professional childcare services for guests with children'),
('Outdoor Terrace', 'Scenic outdoor terrace for relaxation'),
('Bar/Lounge', 'Lounge bar offering a selection of beverages'),
('Concierge Service', 'Concierge assistance for guest convenience'),
('Valet Parking', 'Valet parking services for added convenience'),
('In-Room Safe', 'Secure in-room safes for valuables'),
('Airport Shuttle', 'Convenient airport shuttle services'),
('Bicycle Rental', 'Bicycle rental services for exploring the area'),
('Wheelchair Accessible', 'Wheelchair-accessible rooms and facilities'),
('Fitness Classes', 'Scheduled fitness classes for guests'),
('Library', 'Cozy library with a collection of books'),
('Karaoke Lounge', 'Entertainment lounge with karaoke facilities'),
('Gift Shop', 'On-site gift shop for souvenirs and essentials'),
('Luggage Storage', 'Secure luggage storage facilities'),
('Express Check-In/Check-Out', 'Efficient express check-in and check-out services'),
('Catering Services', 'Event catering services for special occasions'),
('Outdoor Pool Bar', 'Bar service by the outdoor pool'),
('Fireplace in Lobby', 'Warm and welcoming fireplace in the lobby area'),
('Garden Views', 'Rooms with picturesque garden views'),
('Beach Access', 'Direct access to a private beach area'),
('Tea/Coffee Maker', 'In-room tea and coffee making facilities'),
('Game Room', 'Recreation room with various games'),
('Live Entertainment', 'Scheduled live entertainment events'),
('Sun Deck', 'Sunny deck for sunbathing and relaxation'),
('Tennis Courts', 'On-site tennis courts for sports enthusiasts'),
('Guest Lounge', 'Comfortable lounge area for guests'),
('Family-Friendly', 'Family-oriented amenities and services'),
('Roof Terrace', 'Elevated terrace with panoramic views'),
('Outdoor Picnic Area', 'Designated outdoor area for picnics'),
('Steam Room', 'Invigorating steam room facilities'),
('Juice Bar', 'Healthy juice bar with refreshing drinks'),
('Rooftop Garden', 'Lush rooftop garden for tranquility'),
('Wine Cellar', 'In-house wine cellar with a curated selection'),
('Yoga Classes', 'Guided yoga classes for guests'),
('Observation Deck', 'Observation deck with scenic views'),
('Private Cinema', 'Exclusive private cinema for movie nights'),
('Indoor Pool', 'Heated indoor pool for year-round enjoyment'),
('Car Rental Desk', 'On-site car rental desk services'),
('Billiards Room', 'Recreational room with billiards tables'),
('Craft Beer Pub', 'Pub serving a variety of craft beers'),
('Art Gallery', 'On-site art gallery showcasing local artists'),
('Sky Bar', 'Stylish sky bar with cityscape views'),
('Electric Vehicle Charging', 'Charging stations for electric vehicles');

-- Insert  records into the RoomAmenity table (random association between rooms and amenities)
INSERT INTO RoomAmenity (room_id, amenity_id) VALUES
(1, 1), (1, 2),(2, 3), (3, 4), (4, 5), (5, 6), (6, 7),
(7, 8), (8, 9), (9, 10), (10, 11), (11, 12),
(12, 13), (13, 14), (14, 15), (15, 16), (16, 17),
(17, 18), (18, 19), (19, 20), (20, 21), (21, 22),
(22, 23), (23, 24), (24, 25), (25, 26), (26, 27),
(27, 28), (28, 29), (29, 30), (30, 31), (31, 32),
(32, 33), (33, 34), (34, 35), (35, 36), (36, 37),
(37, 38), (38, 39), (39, 40), (40, 41), (41, 42),
(42, 43), (43, 44), (44, 45), (45, 46), (46, 47),
(47, 48), (48, 49), (49, 50), (50, 1);

INSERT INTO HotelAmenity (hotel_id,amenity_id) VALUES
(1, 1), (1, 2),(2, 3), (3, 4), (4, 5), (5, 6), (6, 7),
(7, 8), (8, 9), (9, 10), (10, 11), (11, 12),
(12, 13), (13, 14), (14, 15), (15, 16), (16, 17),
(17, 18), (18, 19), (19, 20), (20, 21), (21, 22),
(22, 23), (23, 24), (24, 25), (25, 26), (26, 27),
(27, 28), (28, 29), (29, 30), (30, 31), (31, 32),
(32, 33), (33, 34), (34, 35), (35, 36), (36, 37),
(37, 38), (38, 39), (39, 40), (40, 41), (41, 42),
(42, 43), (43, 44), (44, 45), (45, 46), (46, 47),
(47, 48), (48, 49), (49, 50);


-- Insert records into the Review table
INSERT INTO Review (reservation_id, rating, comment, review_date) VALUES
(1, 4, 'Great experience!', '2023-05-15'),
(2, 5, 'Amazing service!', '2023-04-12'),
(3, 3, 'Room was okay, could be better.', '2023-06-20'),
(4, 4, 'Enjoyed my stay!', '2023-05-15'),
(5, 5, 'Fantastic hotel!', '2023-04-12'),
(6, 2, 'Disappointing experience.', '2022-11-20'),
(7, 3, 'Average service, needs improvement.', '2023-03-25'),
(8, 5, 'Exceeded expectations!', '2022-12-10'),
(9, 4, 'Lovely atmosphere, great amenities.', '2023-01-18'),
(10, 3, 'Decent stay, could be cleaner.', '2022-10-05'),
(11, 5, 'Perfect getaway!', '2021-09-28'),
(12, 4, 'Impressive service and facilities.', '2021-08-14'),
(13, 2, 'Not recommended, poor service.', '2022-09-22'),
(14, 4, 'Great experience!', '2023-07-08'),
(15, 5, 'Amazing service!', '2023-06-14'),
(16, 3, 'Room was okay, could be better.', '2021-07-19'),
(17, 4, 'Enjoyed my stay!', '2022-02-12'),
(18, 5, 'Fantastic hotel!', '2023-10-30'),
(19, 2, 'Disappointing experience.', '2020-12-05'),
(20, 3, 'Average service, needs improvement.', '2022-01-30'),
(21, 5, 'Exceeded expectations!', '2020-11-15'),
(22, 4, 'Lovely atmosphere, great amenities.', '2022-05-24'),
(23, 3, 'Decent stay, could be cleaner.', '2022-03-07'),
(24, 5, 'Perfect getaway!', '2022-04-18'),
(25, 4, 'Impressive service and facilities.', '2021-05-03'),
(26, 2, 'Not recommended, poor service.', '2023-08-01'),
(27, 4, 'Great experience!', '2022-08-28'),
(28, 5, 'Amazing service!', '2022-07-04'),
(29, 3, 'Room was okay, could be better.', '2021-06-09'),
(30, 4, 'Enjoyed my stay!', '2020-09-12'),
(31, 5, 'Absolutely outstanding!', '2021-11-22'),
(32, 3, 'Mixed feelings about the stay.', '2020-10-14'),
(33, 4, 'Great value for the price.', '2022-06-30'),
(34, 5, 'Exquisite service and amenities.', '2023-09-19'),
(35, 2, 'Not up to expectations.', '2021-12-08'),
(36, 4, 'Nice hotel, friendly staff.', '2022-05-01'),
(37, 5, 'Memorable experience!', '2023-02-25'),
(38, 3, 'Could use some improvements.', '2022-04-02'),
(39, 4, 'Pleasant stay with great views.', '2021-03-15'),
(40, 5, 'Highly recommended!', '2023-08-11'),
(41, 2, 'Disappointing service.', '2020-07-27'),
(42, 4, 'Clean rooms, friendly staff.', '2022-09-06'),
(43, 5, 'Exceptional service!', '2023-01-14'),
(44, 3, 'Average experience overall.', '2020-08-22'),
(45, 4, 'Would stay again!', '2022-03-05'),
(46, 5, 'Perfect for a weekend getaway.', '2021-02-18'),
(47, 2, 'Not the best, but okay.', '2023-04-30'),
(48, 4, 'Beautiful hotel with great views.', '2021-06-19'),
(49, 5, 'Unforgettable stay!', '2022-12-10'),
(50, 3, 'Needs improvement in cleanliness.', '2023-07-08');


-- Insert records into the Payment table
INSERT INTO Payment (reservation_id, amount, payment_date, payment_status) VALUES
(1, 100.00, '2024-01-05', 'Paid'),
(2, 160.00, '2024-02-05', 'Paid'),
(3, 300.00, '2024-03-05', 'Paid'),
(4, 120.00, '2023-04-10', 'Paid'),
(5, 200.00, '2023-05-15', 'Paid'),
(6, 250.00, '2023-06-20', 'Paid'),
(7, 180.00, '2023-07-25', 'Paid'),
(8, 300.00, '2023-08-30', 'Paid'),
(9, 150.00, '2023-09-05', 'Paid'),
(10, 220.00, '2023-10-10', 'Paid'),
(11, 270.00, '2023-11-15', 'Paid'),
(12, 350.00, '2023-12-20', 'Paid'),
(13, 180.00, '2024-01-25', 'Paid'),
(14, 250.00, '2024-02-01', 'Paid'),
(15, 300.00, '2024-03-05', 'Paid'),
(16, 200.00, '2023-04-10', 'Paid'),
(17, 220.00, '2023-05-15', 'Paid'),
(18, 280.00, '2023-06-20', 'Paid'),
(19, 230.00, '2023-07-25', 'Paid'),
(20, 320.00, '2023-08-30', 'Paid'),
(21, 270.00, '2023-09-05', 'Paid'),
(22, 380.00, '2023-10-10', 'Paid'),
(23, 400.00, '2023-11-15', 'Paid'),
(24, 280.00, '2023-12-20', 'Paid'),
(25, 350.00, '2024-01-25', 'Paid'),
(26, 260.00, '2024-02-01', 'Paid'),
(27, 330.00, '2024-03-05', 'Paid'),
(28, 240.00, '2023-04-10', 'Paid'),
(29, 290.00, '2023-05-15', 'Paid'),
(30, 180.00, '2023-06-20', 'Paid'),
(31, 190.00, '2022-07-05', 'Paid'),
(32, 310.00, '2022-08-05', 'Paid'),
(33, 240.00, '2022-09-05', 'Paid'),
(34, 360.00, '2021-10-05', 'Paid'),
(35, 290.00, '2021-11-05', 'Paid'),
(36, 180.00, '2022-12-05', 'Paid'),
(37, 320.00, '2022-01-05', 'Paid'),
(38, 270.00, '2022-02-05', 'Paid'),
(39, 420.00, '2021-03-05', 'Paid'),
(40, 330.00, '2021-04-05', 'Paid'),
(41, 250.00, '2022-05-05', 'Paid'),
(42, 290.00, '2022-06-05', 'Paid'),
(43, 310.00, '2021-07-05', 'Paid'),
(44, 350.00, '2021-08-05', 'Paid'),
(45, 180.00, '2022-09-05', 'Paid'),
(46, 400.00, '2022-10-05', 'Paid'),
(47, 260.00, '2021-11-05', 'Paid'),
(48, 320.00, '2021-12-05', 'Paid'),
(49, 230.00, '2022-01-05', 'Paid'),
(50, 310.00, '2022-02-05', 'Paid');



/* =====================================================================
   PART 2 : ROLE & USER MANAGEMENT UPDATES
   ===================================================================== */

/* ======================================================
   ROLE TABLE
   ====================================================== */

IF OBJECT_ID('Role', 'U') IS NULL
BEGIN
    CREATE TABLE Role (
        role_id INT IDENTITY(1,1) PRIMARY KEY,
        role_name VARCHAR(50) NOT NULL UNIQUE
    );
END;
GO

/* ======================================================
   INSERT DEFAULT ROLES
   ====================================================== */

INSERT INTO Role (role_name)
SELECT 'Admin'
WHERE NOT EXISTS (
    SELECT 1 FROM Role WHERE role_name = 'Admin'
);

INSERT INTO Role (role_name)
SELECT 'User'
WHERE NOT EXISTS (
    SELECT 1 FROM Role WHERE role_name = 'User'
);
GO

/* ======================================================
   USER TABLE
   ====================================================== */

IF OBJECT_ID('[User]', 'U') IS NULL
BEGIN
    CREATE TABLE [User] (
        user_id INT IDENTITY(1,1) PRIMARY KEY,
        full_name VARCHAR(255) NOT NULL,
        email VARCHAR(255) NOT NULL UNIQUE,
        password_hash VARCHAR(MAX) NOT NULL,
        phone VARCHAR(20),
        role_id INT NOT NULL,
        created_at DATETIME DEFAULT GETDATE(),

        CONSTRAINT fk_User_Role
            FOREIGN KEY (role_id)
            REFERENCES Role(role_id)
    );
END;
GO

/* ======================================================
   ADD HOTEL_ID TO ROOM
   ====================================================== */

IF COL_LENGTH('Room', 'hotel_id') IS NULL
BEGIN
    ALTER TABLE Room
    ADD hotel_id INT;
END;
GO

/* ======================================================
   ADD FK ROOM -> HOTEL
   ====================================================== */

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'fk_Room_Hotel'
)
BEGIN
    ALTER TABLE Room
    ADD CONSTRAINT fk_Room_Hotel
    FOREIGN KEY (hotel_id)
    REFERENCES Hotel(hotel_id);
END;
GO

/* ======================================================
   UPDATE EXISTING ROOMS WITH RANDOM HOTELS
   ====================================================== */

UPDATE Room
SET hotel_id =
    CASE
        WHEN room_id BETWEEN 1 AND 10 THEN 1
        WHEN room_id BETWEEN 11 AND 20 THEN 2
        WHEN room_id BETWEEN 21 AND 30 THEN 3
        WHEN room_id BETWEEN 31 AND 40 THEN 4
        ELSE 5
    END;
GO

/* ======================================================
   ADD ROOM STATUS
   ====================================================== */

IF COL_LENGTH('Room', 'room_status') IS NULL
BEGIN
    ALTER TABLE Room
    ADD room_status VARCHAR(50)
    DEFAULT 'Available';
END;
GO

/* ======================================================
   MIGRATE EXISTING is_available VALUES
   ====================================================== */

UPDATE Room
SET room_status =
    CASE
        WHEN is_available = 1 THEN 'Available'
        ELSE 'Reserved'
    END;
GO

/* ======================================================
   ADD RESERVATION STATUS
   ====================================================== */

IF COL_LENGTH('Reservation', 'reservation_status') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD reservation_status VARCHAR(50)
    DEFAULT 'Confirmed';
END;
GO

/* ======================================================
   ADD AUDIT COLUMNS
   ====================================================== */

-- HOTEL
IF COL_LENGTH('Hotel', 'created_at') IS NULL
BEGIN
    ALTER TABLE Hotel
    ADD created_at DATETIME DEFAULT GETDATE();
END;

IF COL_LENGTH('Hotel', 'updated_at') IS NULL
BEGIN
    ALTER TABLE Hotel
    ADD updated_at DATETIME NULL;
END;
GO

-- ROOM
IF COL_LENGTH('Room', 'created_at') IS NULL
BEGIN
    ALTER TABLE Room
    ADD created_at DATETIME DEFAULT GETDATE();
END;

IF COL_LENGTH('Room', 'updated_at') IS NULL
BEGIN
    ALTER TABLE Room
    ADD updated_at DATETIME NULL;
END;
GO

-- RESERVATION
IF COL_LENGTH('Reservation', 'created_at') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD created_at DATETIME DEFAULT GETDATE();
END;

IF COL_LENGTH('Reservation', 'updated_at') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD updated_at DATETIME NULL;
END;
GO

/* ======================================================
   SOFT DELETE COLUMNS
   ====================================================== */

IF COL_LENGTH('Hotel', 'is_deleted') IS NULL
BEGIN
    ALTER TABLE Hotel
    ADD is_deleted BIT DEFAULT 0;
END;
GO

IF COL_LENGTH('Room', 'is_deleted') IS NULL
BEGIN
    ALTER TABLE Room
    ADD is_deleted BIT DEFAULT 0;
END;
GO

IF COL_LENGTH('Reservation', 'is_deleted') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD is_deleted BIT DEFAULT 0;
END;
GO

/* ======================================================
   SAMPLE USERS
   Password = Admin@123
   BCrypt hash generated beforehand
   ====================================================== */

INSERT INTO [User]
(full_name, email, password_hash, phone, role_id)
SELECT
    'System Admin',
    'admin@hotel.com',
    '$2a$11$8m9Y6K1YvQfQ8fR0lV7zXu7pQ5Yv0fL2A8v3JfQz9R2x8wD6WfK8K',
    '9999999999',
    1
WHERE NOT EXISTS (
    SELECT 1 FROM [User]
    WHERE email = 'admin@hotel.com'
);

INSERT INTO [User]
(full_name, email, password_hash, phone, role_id)
SELECT
    'Demo User',
    'user@hotel.com',
    '$2a$11$8m9Y6K1YvQfQ8fR0lV7zXu7pQ5Yv0fL2A8v3JfQz9R2x8wD6WfK8K',
    '8888888888',
    2
WHERE NOT EXISTS (
    SELECT 1 FROM [User]
    WHERE email = 'user@hotel.com'
);
GOdfhE


/* =====================================================================
   PART 3 : RESERVATION USER LINKING
   ===================================================================== */

/* ======================================================
   ADD USER_ID TO RESERVATION
   ====================================================== */

IF COL_LENGTH('Reservation', 'user_id') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD user_id INT NULL;
END;
GO

/* ======================================================
   ADD FOREIGN KEY
   ====================================================== */

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'fk_Reservation_User'
)
BEGIN
    ALTER TABLE Reservation
    ADD CONSTRAINT fk_Reservation_User
    FOREIGN KEY (user_id)
    REFERENCES [User](user_id);
END;
GO

/* ======================================================
   MAKE GUEST FIELDS NOT NULL
   ====================================================== */

ALTER TABLE Reservation
ALTER COLUMN guest_name VARCHAR(255) NOT NULL;
GO

ALTER TABLE Reservation
ALTER COLUMN guest_email VARCHAR(255) NOT NULL;
GO

ALTER TABLE Reservation
ALTER COLUMN guest_phone VARCHAR(20) NOT NULL;
GO

/* ======================================================
   UPDATE EXISTING RESERVATIONS
   Assign all old reservations to Demo User
   ====================================================== */

UPDATE Reservation
SET user_id = 2
WHERE user_id IS NULL;
GO


/* =====================================================================
   PART 4 : FINAL CONSOLIDATED IMPROVEMENTS
   ===================================================================== */

/* =========================================================
   HOTEL MANAGEMENT SYSTEM - COMPLETE UPDATED SQL SCRIPT
   =========================================================
   Includes:
   1. Role-Based Authentication
   2. User Table
   3. Hotel-Room Relationship
   4. Reservation Improvements
   5. Room & Reservation Status
   6. Audit Columns
   7. Soft Delete
   8. Sample Roles
   9. Sample Users
   ========================================================= */


/* =========================================================
   ROLE TABLE
   ========================================================= */

IF OBJECT_ID('Role', 'U') IS NULL
BEGIN
    CREATE TABLE Role (
        role_id INT IDENTITY(1,1) PRIMARY KEY,
        role_name VARCHAR(50) NOT NULL UNIQUE
    );
END;
GO


/* =========================================================
   INSERT DEFAULT ROLES
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM Role WHERE role_name = 'Admin')
BEGIN
    INSERT INTO Role(role_name)
    VALUES ('Admin');
END;
GO

IF NOT EXISTS (SELECT 1 FROM Role WHERE role_name = 'User')
BEGIN
    INSERT INTO Role(role_name)
    VALUES ('User');
END;
GO


/* =========================================================
   USER TABLE
   ========================================================= */

IF OBJECT_ID('[User]', 'U') IS NULL
BEGIN
    CREATE TABLE [User] (
        user_id INT IDENTITY(1,1) PRIMARY KEY,
        full_name VARCHAR(255) NOT NULL,
        email VARCHAR(255) NOT NULL UNIQUE,
        password_hash VARCHAR(MAX) NOT NULL,
        phone VARCHAR(20),
        role_id INT NOT NULL,
        created_at DATETIME DEFAULT GETDATE(),

        CONSTRAINT fk_User_Role
            FOREIGN KEY(role_id)
            REFERENCES Role(role_id)
    );
END;
GO


/* =========================================================
   ADD HOTEL_ID TO ROOM TABLE
   ========================================================= */

IF COL_LENGTH('Room', 'hotel_id') IS NULL
BEGIN
    ALTER TABLE Room
    ADD hotel_id INT;
END;
GO


/* =========================================================
   ROOM -> HOTEL FOREIGN KEY
   ========================================================= */

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'fk_Room_Hotel'
)
BEGIN
    ALTER TABLE Room
    ADD CONSTRAINT fk_Room_Hotel
    FOREIGN KEY (hotel_id)
    REFERENCES Hotel(hotel_id);
END;
GO


/* =========================================================
   ASSIGN EXISTING ROOMS TO HOTELS
   ========================================================= */

UPDATE Room
SET hotel_id =
    CASE
        WHEN room_id BETWEEN 1 AND 10 THEN 1
        WHEN room_id BETWEEN 11 AND 20 THEN 2
        WHEN room_id BETWEEN 21 AND 30 THEN 3
        WHEN room_id BETWEEN 31 AND 40 THEN 4
        ELSE 5
    END
WHERE hotel_id IS NULL;
GO


/* =========================================================
   ROOM STATUS COLUMN
   ========================================================= */

IF COL_LENGTH('Room', 'room_status') IS NULL
BEGIN
    ALTER TABLE Room
    ADD room_status VARCHAR(50)
    DEFAULT 'Available';
END;
GO


/* =========================================================
   MIGRATE OLD is_available DATA
   ========================================================= */

UPDATE Room
SET room_status =
    CASE
        WHEN is_available = 1 THEN 'Available'
        ELSE 'Reserved'
    END
WHERE room_status IS NULL;
GO


/* =========================================================
   RESERVATION STATUS COLUMN
   ========================================================= */

IF COL_LENGTH('Reservation', 'reservation_status') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD reservation_status VARCHAR(50)
    DEFAULT 'Confirmed';
END;
GO


/* =========================================================
   ADD USER_ID TO RESERVATION
   ========================================================= */

IF COL_LENGTH('Reservation', 'user_id') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD user_id INT NULL;
END;
GO


/* =========================================================
   RESERVATION -> USER FOREIGN KEY
   ========================================================= */

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE name = 'fk_Reservation_User'
)
BEGIN
    ALTER TABLE Reservation
    ADD CONSTRAINT fk_Reservation_User
    FOREIGN KEY(user_id)
    REFERENCES [User](user_id);
END;
GO


/* =========================================================
   MAKE GUEST DETAILS REQUIRED
   ========================================================= */

ALTER TABLE Reservation
ALTER COLUMN guest_name VARCHAR(255) NOT NULL;
GO

ALTER TABLE Reservation
ALTER COLUMN guest_email VARCHAR(255) NOT NULL;
GO

ALTER TABLE Reservation
ALTER COLUMN guest_phone VARCHAR(20) NOT NULL;
GO


/* =========================================================
   UPDATE OLD RESERVATIONS
   Assign Existing Reservations to Demo User
   ========================================================= */

UPDATE Reservation
SET user_id = 2
WHERE user_id IS NULL;
GO


/* =========================================================
   AUDIT COLUMNS - HOTEL
   ========================================================= */

IF COL_LENGTH('Hotel', 'created_at') IS NULL
BEGIN
    ALTER TABLE Hotel
    ADD created_at DATETIME DEFAULT GETDATE();
END;
GO

IF COL_LENGTH('Hotel', 'updated_at') IS NULL
BEGIN
    ALTER TABLE Hotel
    ADD updated_at DATETIME NULL;
END;
GO


/* =========================================================
   AUDIT COLUMNS - ROOM
   ========================================================= */

IF COL_LENGTH('Room', 'created_at') IS NULL
BEGIN
    ALTER TABLE Room
    ADD created_at DATETIME DEFAULT GETDATE();
END;
GO

IF COL_LENGTH('Room', 'updated_at') IS NULL
BEGIN
    ALTER TABLE Room
    ADD updated_at DATETIME NULL;
END;
GO


/* =========================================================
   AUDIT COLUMNS - RESERVATION
   ========================================================= */

IF COL_LENGTH('Reservation', 'created_at') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD created_at DATETIME DEFAULT GETDATE();
END;
GO

IF COL_LENGTH('Reservation', 'updated_at') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD updated_at DATETIME NULL;
END;
GO


/* =========================================================
   SOFT DELETE - HOTEL
   ========================================================= */

IF COL_LENGTH('Hotel', 'is_deleted') IS NULL
BEGIN
    ALTER TABLE Hotel
    ADD is_deleted BIT DEFAULT 0;
END;
GO


/* =========================================================
   SOFT DELETE - ROOM
   ========================================================= */

IF COL_LENGTH('Room', 'is_deleted') IS NULL
BEGIN
    ALTER TABLE Room
    ADD is_deleted BIT DEFAULT 0;
END;
GO


/* =========================================================
   SOFT DELETE - RESERVATION
   ========================================================= */

IF COL_LENGTH('Reservation', 'is_deleted') IS NULL
BEGIN
    ALTER TABLE Reservation
    ADD is_deleted BIT DEFAULT 0;
END;
GO


/* =========================================================
   SAMPLE ADMIN USER
   Password: Admin@123
   ========================================================= */

IF NOT EXISTS (
    SELECT 1 FROM [User]
    WHERE email = 'admin@hotel.com'
)
BEGIN
    INSERT INTO [User]
    (
        full_name,
        email,
        password_hash,
        phone,
        role_id
    )
    VALUES
    (
        'System Admin',
        'admin@hotel.com',
        '$2a$11$8m9Y6K1YvQfQ8fR0lV7zXu7pQ5Yv0fL2A8v3JfQz9R2x8wD6WfK8K',
        '9999999999',
        1
    );
END;
GO


/* =========================================================
   SAMPLE NORMAL USER
   Password: User@123
   ========================================================= */

IF NOT EXISTS (
    SELECT 1 FROM [User]
    WHERE email = 'user@hotel.com'
)
BEGIN
    INSERT INTO [User]
    (
        full_name,
        email,
        password_hash,
        phone,
        role_id
    )
    VALUES
    (
        'Demo User',
        'user@hotel.com',
        '$2a$11$8m9Y6K1YvQfQ8fR0lV7zXu7pQ5Yv0fL2A8v3JfQz9R2x8wD6WfK8K',
        '8888888888',
        2
    );
END;
GO


/* =========================================================
   VERIFY DATA
   ========================================================= */

SELECT * FROM Role;
SELECT * FROM [User];
SELECT * FROM Hotel;
SELECT * FROM Room;
SELECT * FROM Reservation;
GO
