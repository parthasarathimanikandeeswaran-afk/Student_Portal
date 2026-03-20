select * FROM StudentMarks;
select * FROM StudentResults;

-- 2️⃣ Delete parent table
select * FROM Students;

-- 3️⃣ Reset identity (auto-increment IDs)
DBCC CHECKIDENT ('StudentMarks', RESEED, 0);
DBCC CHECKIDENT ('StudentResult', RESEED, 0);
DBCC CHECKIDENT ('Students', RESEED, 0);
