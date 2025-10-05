-- Delete the admin user rows (UserId 4 and 6)
DELETE FROM Users WHERE UserId IN (4, 6);
-- or
DELETE FROM Users WHERE Email = 'admin@qardx.com';