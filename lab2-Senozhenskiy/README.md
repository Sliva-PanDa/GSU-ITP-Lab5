[![Build Lab 2 Project (Senozhenskiy)](https://github.com/IT-GSTU/itp16/actions/workflows/lab2-build.yml/badge.svg)](https://github.com/IT-GSTU/itp16/actions/workflows/lab2-build.yml)

# Лабораторная работа №2: Использование Entity Framework и LINQ

**Студент:** Сеноженский В.В.  
**Группа:** ИТП-31  
**Вариант:** 16

## Описание

Консольное приложение на .NET Core для взаимодействия с базой данных MS SQL Server. 
В рамках работы реализованы:
- Подключение к удаленной БД с использованием строки подключения из `appsettings.json`.
- Генерация моделей с помощью EF Core (подход Database First).
- Выполнение LINQ-запросов (выборка, фильтрация, группировка, CRUD-операции).
- Настройка рабочего процесса GitHub Actions для автоматической сборки проекта.