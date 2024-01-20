using Microsoft.EntityFrameworkCore;
using SAViE.Commands;
using SAViE.Models;
using SAViE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace SAViE.ViewModels
{
    class KonspektViewModel : ViewModelBase
    {
        #region variables
        private List<Notes>? notesList;
        public List<Notes>? NotesList
        {   
            get => notesList;
            set => Set(ref notesList, value);
        }

        private List<Topic>? topicList;
        public List<Topic> TopicList
        {
            get => topicList;
            set => Set(ref topicList, value);
        }

        private List<Topic>? selectedNoteTopicList;
        public List<Topic> SelectedNoteTopicList
        {
            get => selectedNoteTopicList;
            set => Set(ref selectedNoteTopicList, value);
        }

        private string noteName = "Введите название";
        public string NoteName
        {
            get => noteName;
            set => Set(ref noteName, value);
        }

        private string topicText;
        public string TopicText
        {
            get => topicText;
            set => Set(ref topicText, value);
        }

        private Notes? selectedNote;
        public Notes SelectedNote 
        {
            get => selectedNote;
            set => Set(ref selectedNote, value);
        }

        private Topic? selectedTopic;
        public Topic SelectedTopic
        {
            get => selectedTopic;
            set => Set(ref selectedTopic, value);
        }
        #endregion

        #region EFCreateNote
        public ICommand EFCreateNote { get; set; }
        private bool CanEFCreateNoteExecute(object p) => true;
        private void OnEFCreateNoteExecuted(object p)
        {
            using (DataContext context = new DataContext())
            {
                context.NotesSq.Add(new Notes { Name = NoteName });
                context.SaveChanges();
                EFRead();
            }
        }
        #endregion

        #region EFCreateTopic
        public ICommand EFCreateTopic { get; set; }
        private bool CanEFCreateTopicExecute(object p) => true;
        private void OnEFCreateTopicExecuted(object p)
        {
            using (DataContext context = new DataContext())
            {
                if (SelectedNote != null)
                {
                    context.TopicSq.Add(new Topic() { Name = NoteName, Text = "", NoteId = SelectedNote.ID });
                    context.SaveChanges();
                    SelectedNoteTopicList = context.TopicSq.Where(x => x.NoteId == SelectedNote.ID).ToList();
                    EFRead();
                    MessageBox.Show("Не забудь выбрать конспект, а то у тебя ничего не сохранится ^_^");
                }
                else
                    MessageBox.Show("Выберите конспект в который хотите добавить тему");
            }
        }
        #endregion

        #region EFUpdate
        public ICommand EFUpdate { get; set; }
        private bool CanEFUpdateExecute(object p) => true;
        private void OnEFUpdateExecuted(object p)
        {
            using (DataContext context = new DataContext())
            {
                Topic? selectedtopic = p as Topic;
                if (SelectedNote != null && SelectedTopic != null)
                {
                    Topic topic = context.TopicSq.Find(selectedTopic.ID);
                    topic.Text = TopicText;
                    context.SaveChanges();//менятся текст в базе
                    SelectedNoteTopicList.FirstOrDefault(x => x.ID == topic.ID).Text = topic.Text;
                    /* SelectedNoteTopicList[SelectedTopic.ID - 1].Text = topic.Text;*///меняется текст в selectedNoteTopicList
                }
                else
                    MessageBox.Show("Посмотри выделил ли ты конспект или тему ^_^");
            }
        }
        #endregion

        #region EFDeleteNote
        public ICommand EFDeleteNote { get; set; }
        private bool CanEFDeleteNoteExecute(object p) => true;
        private void OnEFDeleteNoteExecuted(object p)
        {
            using (DataContext context = new DataContext())
            {
                var result = MessageBox.Show("Ты правда хочешь его убить...?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                /*Notes? selectedNote = p as Notes;*/
                if (result == MessageBoxResult.Yes && SelectedTopic != null)
                {
                    Topic topicToDel = context.TopicSq.Find(SelectedTopic.ID);
                    SelectedNoteTopicList.RemoveAt(SelectedNoteTopicList.Count(x => x.ID == SelectedTopic.ID) - 1);
                    TopicList.RemoveAt(SelectedNoteTopicList.Count(x => x.ID == SelectedTopic.ID));
                    context.Remove(topicToDel);
                    context.SaveChanges();
                    SelectedNoteTopicList = TopicList.Where(x => x.NoteId == SelectedNote.ID).ToList();
                }
                else if (result == MessageBoxResult.Yes && SelectedNote != null)
                {
                    while(SelectedNoteTopicList.Count() != 0)
                    {
                        SelectedNoteTopicList.RemoveAt(0);
                        Topic topToDel = context.TopicSq.FirstOrDefault(x => x.NoteId == SelectedNote.ID); 
                        context.Remove(topToDel);
                        context.SaveChanges();
                    }
                    Notes noteToDel = context.NotesSq.Find(SelectedNote.ID);
                    context.Remove(noteToDel);
                    context.SaveChanges();
                }
                EFRead();
            }
        }
        #endregion

        #region TopicSelected
        public ICommand TopicSelected { get; set; }
        private bool CanTopicSelectedExecute(object p) => true;
        private void OnTopicSelectedExecuted(object p)
        {
            if (p != null)
            {
                Topic? topic = p as Topic;
                TopicText = topic.Text;
            }
        }
        #endregion

        #region NoteSelected
        public ICommand NoteSelected { get; set; }
        private bool CanNoteSelectedExecute(object p) => true;
        private void OnNoteSelectedExecuted(object p)
        {
            if (SelectedNote != null)
            {
                SelectedNoteTopicList = TopicList.Where(x => x.NoteId == SelectedNote.ID).ToList();
                SelectedTopic = null;
                TopicText = "Выберите тему или создайте новую\n\n　　　　　　　　 　　　　　　／ ¯¯｀フ\r\n　　　　　　　　　,　'' ｀ ｀ / 　 　 　 !　 　\r\n　　　　　　　 , ' 　　　　 レ　 _,　 -' ミ\r\n　　　　　　　 ; 　 　 　 　 　`ミ __,xノﾞ､\r\n　　　 　　 　 i　 　　　ﾐ　　　; ,､､､、　ヽ ¸\r\n　　　 　　,.-‐! 　 　 　 ﾐ　　i　　　　｀ヽ.._,,))\r\n　　 　　//´｀｀､　　　　 ミ　ヽ　　　　　(¯`v´¯)\r\n　　　　| l　　 　｀ ｰｰ -‐''ゝ､,,)).　 　　 　 ..`·.¸.·´\r\n　　　 　ヽ.ー─'´)　";
            }
        }
        #endregion

        public void EFRead()
        {
            using (DataContext context = new DataContext())
            {
                NotesList = context.NotesSq.ToList();
                TopicList = context.TopicSq.ToList();
            }
        }

        public KonspektViewModel()
        {
            EFRead();
            NoteSelected = new RelayCommand(OnNoteSelectedExecuted, CanNoteSelectedExecute);
            TopicSelected = new RelayCommand(OnTopicSelectedExecuted, CanTopicSelectedExecute);
            EFUpdate = new RelayCommand(OnEFUpdateExecuted, CanEFUpdateExecute);
            EFCreateNote = new RelayCommand(OnEFCreateNoteExecuted, CanEFCreateNoteExecute);
            EFCreateTopic = new RelayCommand(OnEFCreateTopicExecuted, CanEFCreateTopicExecute);
            EFDeleteNote = new RelayCommand(OnEFDeleteNoteExecuted, CanEFDeleteNoteExecute);
        }
    }
}
