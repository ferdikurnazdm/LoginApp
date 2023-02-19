/*********************************************** Project Information **************************************************************
 *
 * project name : saved folder
 * Company name : ------------
 * Date         : 19.02.2023
 * Writed by    : Ferdi Kurnaz
 * 
 * *******************************************************************************************************************************
 * 
 * Description  : Burada ilk olarak app config dosyasından _passWord ve _filePath key'lerini okuyoruz. Daha sonra password
 * kontrolü yapıyoruz. pasaport doğru ise klasörün kilidini kaldırıyoruz. kullanıcı yapmak istediği yapıyor daha sonra 
 * uygulamayı kapatmak istediğinde kullanıcı x işaretine tıkladığı zaman öcde klasör kilitleniyor daha sonra uygulama kapanıyor.
 * 
 * *******************************************************************************************************************************
 * 
 * Requests     : 
 * - app config _filePath --> kilitlenecek klasörün dosya dizini (string)
 * - app config _passWord --> giriş yapmak için gereken şifre (string)
 * 
 * *******************************************************************************************************************************/
using System;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Diagnostics;

namespace LoginApp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// hangi field neyi saklıyor.
        /// _passWord --> kullanıcı şifresini tutar.
        /// _filePath --> kilitlenecek klasör dizinini tutar.
        /// _userNema --> hangi Windows oturumu için geçerli olacağını tutar.
        /// </summary>
        private static string _passWord;
        private static string _filePath;
        private static string _userName;
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// burada uygulama açılır açılmaz yağılacak işlemler yer almaktadır.
        /// ek olarak _userName sistem tarafından otomatoik çekilmektedir.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            btn_openfolder.Enabled = false;
            _passWord = ConfigurationSettings.AppSettings["_passWord"];
            _filePath = ConfigurationSettings.AppSettings["_filePath"];
            _userName=Environment.UserName;
        }
        /// <summary>
        /// Burada biz password kontrolü yapıyoruz. 
        /// eğer pasaport doğru ise klasörün kilidini açıyoruz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Openlock_Click(object sender, EventArgs e)
        {
            if (textBox1.Text==_passWord)
            {
                DirectorySecurity security = Directory.GetAccessControl(_filePath);
                FileSystemAccessRule rule = new FileSystemAccessRule(_userName, FileSystemRights.FullControl, AccessControlType.Deny);
                security.RemoveAccessRule(rule);
                Directory.SetAccessControl(_filePath, security);
                MessageBox.Show("Unlocked");
                btn_openfolder.Enabled = true;
            }
            else if (textBox1.Text=="")
            {
                MessageBox.Show("password is empty!");
            }
            else { MessageBox.Show("password is wrong!"); }
        }
        /// <summary>
        /// burada kullanıcı kesin olarak  uygulamayı kapatıcak.
        /// bizde kullanıcı uygulamayı kapatırken klasörü tekrardan kilitleyeceğiz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            DirectorySecurity security = Directory.GetAccessControl(_filePath);
            FileSystemAccessRule rule = new FileSystemAccessRule(_userName, FileSystemRights.FullControl, AccessControlType.Deny);
            security.AddAccessRule(rule);
            Directory.SetAccessControl(_filePath, security);
            MessageBox.Show("Locked");
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// open folder metodunda kullanıcı şifreyi girip klösörün kilidini kaldırdığı zaman.
        /// open foldeer butonuna tıkladığında klasör dizinine gitmekle uğraşmadan direkt
        /// karşısına gelecek.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_openfolder_Click(object sender, EventArgs e)
        {
            Process.Start(_filePath);
        }
    }
}
