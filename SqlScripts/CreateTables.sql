DO $$
BEGIN
    CREATE TABLE Surveys (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    description TEXT,
    status VARCHAR(20) NOT NULL CHECK (Status IN ('active', 'inactive'))
    );

    CREATE TABLE Questions (
        Id SERIAL PRIMARY KEY,
        survey_id INT NOT NULL,
        question_text TEXT NOT NULL,
        display_order INT NOT NULL,
        CONSTRAINT fk_question_survey 
            FOREIGN KEY (survey_id) 
            REFERENCES Surveys(id) 
            ON DELETE CASCADE
    );

    CREATE TABLE Answers (
        id SERIAL PRIMARY KEY,
        question_id INT NOT NULL,
        answer_text TEXT NOT NULL,
        display_order INT NOT NULL,
        CONSTRAINT fk_answer_question 
            FOREIGN KEY (question_id) 
            REFERENCES Questions(id) 
            ON DELETE CASCADE
    );

    CREATE TABLE Interviews (
        id SERIAL PRIMARY KEY,
        survey_id INT NOT NULL,
        start_time TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
        end_time TIMESTAMPTZ,
        status VARCHAR(20) NOT NULL 
            CHECK (status IN ('in_progress', 'completed', 'abandoned')),
        CONSTRAINT fk_interview_survey 
            FOREIGN KEY (survey_id) 
            REFERENCES Surveys(id) 
            ON DELETE CASCADE
    );

    CREATE TABLE Results (
        id SERIAL PRIMARY KEY, -- можно использовать BIGSERIAL для больших объемов данных
        interview_id INT NOT NULL,
        question_id INT NOT NULL,
        answer_id INT,
        text_response TEXT,
        response_time TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
        CONSTRAINT fk_result_interview 
            FOREIGN KEY (interview_id) 
            REFERENCES Interviews(id) 
            ON DELETE CASCADE,
        CONSTRAINT fk_result_question 
            FOREIGN KEY (question_id) 
            REFERENCES Questions(id) 
            ON DELETE CASCADE,
        CONSTRAINT fk_result_answer 
            FOREIGN KEY (answer_id) 
            REFERENCES Answers(id) 
            ON DELETE SET NULL,
        CONSTRAINT chk_result_data 
            CHECK (
                (answer_id IS NOT NULL AND text_response IS NULL) OR 
                (answer_id IS NULL AND text_response IS NOT NULL)
            )
    );

    -- Добавление уникальных индексов
    CREATE UNIQUE INDEX uidx_question_survey_order 
    ON Questions (survey_id, display_order);

    CREATE UNIQUE INDEX uidx_answer_question_order 
    ON Answers (question_id, display_order);
END
$$;