CREATE TABLE reports (
    report_id INTEGER PRIMARY KEY,
	company_id INTEGER NOT NULL,
	task_id INTEGER NOT NULL,
	create_date TEXT,
	content TEXT
);
