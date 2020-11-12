DROP table onetimeloan;
DROP table udlaant;
DROP table pc;
Drop Table laaner;
DROP procedure _aflever;
Drop procedure _udlaan;
Drop procedure tlf_loan;
Drop procedure tlf_return;

CREATE TABLE laaner(
	login varchar(16) primary key,
	laanerID varchar(16),
	name varchar(255),
	tlf_Number varchar(8),
	is_student bool
);

CREATE TABLE pc(
	QR_ID varchar(16) primary Key,
	loebe_nummer varchar(16),
	pc_model varchar(255),
	in_stock bool
);

CREATE TABLE udlaant(
	login varchar(16) references laaner(login),
	qr_id varchar(16) references pc(QR_ID),
	start_date date,
	end_date date
);
CREATE TABLE OneTimeLoan(
	phone_number varchar(8) primary key,
	qr_id varchar(16) references pc(QR_ID),
	date_of_loan date
);

CREATE or REPLACE PROCEDURE tlf_loan(
	tlf_number varchar(8),
	qr_id_pc varchar(16),
	todays_date date
)
language plpgsql as $$
begin
	INSERT INTO OneTimeLoan VALUES (tlf_number, qr_id_pc, todays_date);
	UPDATE pc SET in_stock = false WHERE QR_ID = qr_id_pc;
	commit;
end;$$;

CREATE or REPLACE PROCEDURE tlf_return(
	qr_id_pc Varchar(16)
)
Language plpgsql as $$
begin
	Delete from OneTimeLoan Where QR_id = qr_id_pc;
	Update pc set in_stock = true Where QR_ID = qr_id_pc;
	commit;
end;$$;

CREATE or REPLACE PROCEDURE _Udlaan(
	laanerID varchar(16),
	qr_id_pc varchar(16),
	start_date date,
	end_date date
)
language plpgsql as $$
begin
	insert into Udlaant Values (laanerID, qr_id_pc, start_date, end_date);
	Update pc set in_stock = false Where QR_ID = qr_id_pc;
	commit;
end;$$;

CREATE or REPLACE PROCEDURE _aflever(
	laanerID varchar(16),
	qr_id_pc varchar(16)
)
language plpgsql as $$
begin
	Delete From laaner where QR_ID = qr_id_pc;
	Update pc set in_stock = true Where QR_ID = qr_id_pc;
	commit;
end;$$;