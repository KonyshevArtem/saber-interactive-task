# Тестовое задание Saber Interactive на должность Gameplay Programmer [![Build Status](https://travis-ci.com/KonyshevArtem/saber-interactive-task.svg?token=z3sjoAcC4HGWpiWtgTxy&branch=master)](https://travis-ci.com/KonyshevArtem/saber-interactive-task)

## Задание 1

Реализуйте функции сериализации и десериализации двусвязного списка, заданного следующим образом:

    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }


    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
        }

        public void Deserialize(FileStream s)
        {
        }
    }

Примечание: сериализация подразумевает сохранение и восстановление полной структуры списка, включая взаимное соотношение его элементов между собой.  Формат сериализованного файла любой.
* Нельзя изменять исходную структуру классов ListNode, ListRand.
* **Алгоритмическая сложность решения должна быть меньше квадратичной.**
* Для выполнения задания можно использовать любой общеиспользуемый язык.
* Тест нужно выполнить без использования библиотек/стандартных средств сериализации.

## Задание 2

Составить BehaviourTree (в виде схемы) для NPC, который:
* в спокойном состоянии патрулирует территорию;
* по окончанию каждого патруля (окончанием считается возвращение в стартовую точку) NPC может или заснуть на 2 минуты или продолжить патруль;
* когда у NPC есть враг, он  атакует его, причем стрелять во врага может, только если у него есть патроны и враг на расстоянии меньше 30 метров;
* если патронов нет, то NPC убегает от врага.
