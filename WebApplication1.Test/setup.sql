-- Changeset Migrations/2019-06-03-01-create-departments-table.xml::2019-06-03-01-create-departments-table::tolstovku
CREATE TABLE public.departments (id SERIAL NOT NULL, name TEXT NOT NULL, CONSTRAINT DEPARTMENTS_PKEY PRIMARY KEY (id));

-- Changeset Migrations/2019-06-03-02-create-roles-table.xml::2019-06-03-02-create-roles-table::tolstovku
CREATE TABLE public.roles (id SERIAL NOT NULL, name TEXT NOT NULL, CONSTRAINT ROLES_PKEY PRIMARY KEY (id));

-- Changeset Migrations/2019-06-03-03-create-users-table.xml::2019-06-03-03-create-users-table::tolstovku
CREATE TABLE public.users (id SERIAL NOT NULL, first_name TEXT NOT NULL, last_name TEXT NOT NULL, patronymic TEXT, nickname TEXT NOT NULL, email TEXT NOT NULL, password TEXT NOT NULL, phone_number TEXT, invited_at TIMESTAMP WITHOUT TIME ZONE NOT NULL, description TEXT, department_id INTEGER NOT NULL, CONSTRAINT USERS_PKEY PRIMARY KEY (id));

ALTER TABLE public.users ADD CONSTRAINT fk_users_department__departments FOREIGN KEY (department_id) REFERENCES public.departments (id) ON UPDATE RESTRICT ON DELETE CASCADE;

-- Changeset Migrations/2019-06-03-04-create-user-roles-table.xml::2019-06-03-04-create-user_roles-table::tolstovku
CREATE TABLE public.user_roles (user_id INTEGER NOT NULL, role_id INTEGER NOT NULL);

ALTER TABLE public.user_roles ADD CONSTRAINT fk_user_roles_user__users FOREIGN KEY (user_id) REFERENCES public.users (id) ON UPDATE RESTRICT ON DELETE CASCADE;

ALTER TABLE public.user_roles ADD CONSTRAINT fk_user_roles_role__roles FOREIGN KEY (user_id) REFERENCES public.roles (id) ON UPDATE RESTRICT ON DELETE CASCADE;

ALTER TABLE public.user_roles ADD PRIMARY KEY (user_id, role_id);

-- Changeset Migrations/2019-06-03-05-create-user-managers-table.xml::2019-06-03-05-create-user_managers-table::tolstovku
CREATE TABLE public.user_managers (user_id INTEGER NOT NULL, manager_id INTEGER NOT NULL);

ALTER TABLE public.user_managers ADD PRIMARY KEY (user_id, manager_id);

ALTER TABLE public.user_managers ADD CONSTRAINT fk_user_managers_user__users FOREIGN KEY (user_id) REFERENCES public.users (id) ON UPDATE RESTRICT ON DELETE CASCADE;

ALTER TABLE public.user_managers ADD CONSTRAINT fk_user_managers_manager__users FOREIGN KEY (manager_id) REFERENCES public.users (id) ON UPDATE RESTRICT ON DELETE CASCADE;

-- Changeset Migrations/2019-06-03-06-create-salary-rates-table.xml::2019-06-03-06-create-salary_rates-table::tolstovku
CREATE TABLE public.salary_rates (id SERIAL NOT NULL, rate INTEGER NOT NULL, updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL, user_id INTEGER NOT NULL, description TEXT, CONSTRAINT SALARY_RATES_PKEY PRIMARY KEY (id));

ALTER TABLE public.salary_rates ADD CONSTRAINT fk_salary_rates_user__users FOREIGN KEY (user_id) REFERENCES public.users (id) ON UPDATE RESTRICT ON DELETE CASCADE;

-- Changeset Migrations/2019-06-03-07-create-salary-rate-requests-table.xml::2019-06-03-07-create-salary_rate_requests-table::tolstovku
CREATE TABLE public.salary_rate_requests (id SERIAL NOT NULL, request_chain_id SERIAL NOT NULL, suggested_rate INTEGER NOT NULL, status INTEGER NOT NULL, created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL, reviewer_id INTEGER, sender_id INTEGER NOT NULL, reviewer_comment TEXT, internal_comment TEXT, reason TEXT NOT NULL, CONSTRAINT SALARY_RATE_REQUESTS_PKEY PRIMARY KEY (id));

ALTER TABLE public.salary_rate_requests ADD CONSTRAINT fk_salary_rate_requests_sender_id__users FOREIGN KEY (sender_id) REFERENCES public.users (id) ON UPDATE RESTRICT ON DELETE CASCADE;

ALTER TABLE public.salary_rate_requests ADD CONSTRAINT fk_salary_rate_requests_reviewer_id__users FOREIGN KEY (reviewer_id) REFERENCES public.users (id) ON UPDATE RESTRICT ON DELETE CASCADE;

-- Changeset Seeds/2019-06-03-08-insert-departments.xml::2019-06-03-08-insert-departments::tolstovku
INSERT INTO public.departments (name) VALUES ('Frontend');

INSERT INTO public.departments (name) VALUES ('Backend');

-- Changeset Seeds/2019-06-03-09-insert-users.xml::2019-06-03-09-insert-users::tolstovku
INSERT INTO public.users (nickname, password, first_name, last_name, email, department_id, invited_at) VALUES ('Admin', 'AQAAAAEAACcQAAAAEDIQZRJJvqkOh8aXCFtup1SEObbC6295iV8fjic0Dxql73jLe0PsXSoHMX5As75p5Q==', 'Vlad', 'nimatora', 'tatata@yandex.ru', '2', '01.01.2019');

INSERT INTO public.users (nickname, password, first_name, last_name, email, department_id, invited_at) VALUES ('User', 'AQAAAAEAACcQAAAAEKpYbzc4YZbIZIJYd/F1XI8qhGjStan+x146+FEphPW5vIjwM1vBuPSc27WuUczrPw==', 'Dan', 'Tolstovku', 'shitmail@yandex.ru', '1', '01.01.2019');

INSERT INTO public.users (nickname, password, first_name, last_name, email, department_id, invited_at) VALUES ('Manager', 'AQAAAAEAACcQAAAAEO5TgbvMXgk7K9i3kfaXaTF6556z6S+WLzUyx3PB+h/4v333ROtm5n/Lbu6GExNwAw==', 'Someone', 'Something', 'managerEmail@yandex.ru', '2', '01.01.2019');

-- Changeset Seeds/2019-06-03-10-insert-roles.xml::2019-06-03-10-insert-roles::tolstovku
INSERT INTO public.roles (name) VALUES ('user');

INSERT INTO public.roles (name) VALUES ('manager');

INSERT INTO public.roles (name) VALUES ('admin');

-- Changeset Seeds/2019-06-03-11-insert-user-roles.xml::2019-06-03-11-insert-user_roles::tolstovku
INSERT INTO public.user_roles (user_id, role_id) VALUES ('1', '3');

INSERT INTO public.user_roles (user_id, role_id) VALUES ('2', '1');

INSERT INTO public.user_roles (user_id, role_id) VALUES ('3', '2');

-- Changeset Seeds/2019-06-03-12-insert-salary-rate-requests.xml::2019-06-03-12-insert-salary_rate_requests::tolstovku
INSERT INTO public.salary_rate_requests (request_chain_id, suggested_rate, reason, sender_id, status, created_at) VALUES ('1', '1337', 'Want money', '2', '3', '01.01.2019');

INSERT INTO public.salary_rate_requests (request_chain_id, suggested_rate, reason, sender_id, status, created_at) VALUES ('2', '1427', 'I also want money', '3', '3', '01.01.2019');

-- Changeset Seeds/2019-06-03-13-insert-user-managers.xml::2019-06-03-13-insert-user_managers::tolstovku
INSERT INTO public.user_managers (user_id, manager_id) VALUES ('2', '3');
