using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMHookingUpViews.Model;
using System.Windows.Input;

namespace MVVMHookingUpViews.ViewModel
{
    public class StudentViewModel
    {
        public ICommand CreateCommand { get; set; }

        public ICommand RemoveCommand { get; set; }
        private static int newStudentCount = 0;
        public ObservableCollection<StudentModel> Students
        {
            get;
            set;
        }
        public ObservableCollection<StudentModel> students = new ObservableCollection<StudentModel>();
        public StudentViewModel()
        {
            students.Add(new StudentModel { FirstName = "Mark", LastName = "Allain" });
            students.Add(new StudentModel { FirstName = "Allen", LastName = "Brown" });
            students.Add(new StudentModel { FirstName = "Linda", LastName = "Hamerski" });
            LoadStudents();
            CreateCommand = new MyICommand(CreateStudent);
            RemoveCommand = new MyICommand(CanRemoveRow, RemoveRow);
     
        }

        public void LoadStudents()
        {
            Students = students;
        }

        // Create Stuent with default First Name and Last name
        public void CreateStudent(object parameter)
        {
            try
            {
                newStudentCount++;
                students.Add(new StudentModel { FirstName = "NewFirst "+ newStudentCount, LastName = "NewLast "+ newStudentCount });
                LoadStudents();
            }
            catch (Exception ex)
            {

            }
        }

// Code for delete individual Row
        private void RemoveRow(object parameter)
        {
            int index = Students.IndexOf(parameter as StudentModel);
            if (index > -1 && index < Students.Count)
            {
                Students.RemoveAt(index);
            }
        }
        private bool CanRemoveRow(object parameter)
        {
            return true;
        }
    }
}
