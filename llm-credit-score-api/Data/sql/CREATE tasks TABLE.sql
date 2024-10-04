CREATE TABLE tasks (
    task_id INTEGER PRIMARY KEY,
	task_key TEXT,
	status TEXT,
	company_id INTEGER NOT NULL,
	report_id INTEGER NULL,
	create_date TEXT
);
