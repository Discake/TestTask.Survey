echo Starting SQL script execution...
docker exec -i testtask.survey.db psql -U admin -d surveydb -f - < ClearData.sql
if %errorlevel% equ 0 (
    echo Script executed successfully.
) else (
    echo Script execution failed.
)