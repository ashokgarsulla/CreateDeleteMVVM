
using System.Windows.Input;
using MVVMHookingUpViews.Model;
using MVVMHookingUpViews.Commands;
using System.Collections.ObjectModel;

namespace MVVMHookingUpViews.ViewModel
{
    public class LeftViewModel
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

        public LeftViewModel()
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
        public void CreateStudent(object parameter)
        {
            newStudentCount++;
            students.Add(new StudentModel { FirstName = "NewFirst " + newStudentCount, LastName = "NewLast " + newStudentCount });
            LoadStudents();
        }

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
