SELECT session_id, login_name, host_name
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('QardXDB');
