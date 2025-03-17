DO $$
BEGIN
    -- Создаем тестовую анкету
    INSERT INTO surveys (title, description, status)
    VALUES (
        'Тестовая анкета для проверки индексов',
        'Анкета с большим количеством вопросов и ответов',
        'active'
    );
    DECLARE
        survey_id INT;
        question_id INT;
        num_questions INT := 1000; -- Количество вопросов
        num_answers_per_question INT := 10; -- Количество ответов на каждый вопрос
    BEGIN

        -- Получаем ID созданной анкеты
        SELECT id INTO survey_id FROM surveys LIMIT 1;

        -- Генерация вопросов
        FOR i IN 1..num_questions LOOP
            INSERT INTO questions (survey_id, question_text, display_order)
            VALUES (
                survey_id,
                'Вопрос ' || i,
                i
            )
            RETURNING id INTO question_id;

            -- Генерация ответов для вопроса

            FOR j IN 1..num_answers_per_question LOOP
                INSERT INTO answers (question_id, answer_text, display_order)
                VALUES (
                    question_id,
                    'Ответ ' || j || ' на вопрос ' || i,
                    j
                );
            END LOOP;

        END LOOP;
    END;
END
$$;