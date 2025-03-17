DO $$
BEGIN
    -- Очистка таблиц (опционально)
    TRUNCATE TABLE results, interviews, answers, questions, surveys RESTART IDENTITY CASCADE;
END
$$;