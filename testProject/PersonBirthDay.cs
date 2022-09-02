using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testProject
{
    /// <summary>
    /// Класс для хранения ФИ и Даты рождения
    /// </summary>
    class PersonBirthDay
    {
        //Fields
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }

        //Constructors
        public PersonBirthDay() { }
        public PersonBirthDay(string name, string surname, DateTime birthDay)
        {
            Surname = surname;
            Name = name;
            BirthDay = birthDay;
        }

        //methods
        public override string ToString()
        {
            return Surname + " " + Name + " " + BirthDay.ToString();
        }
    }
}
