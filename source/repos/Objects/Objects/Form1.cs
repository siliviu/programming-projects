using System;
using System.Drawing;
using System.Windows.Forms;

namespace Objects
{
    public partial class Form1 : Form
    {
        public Panel[] TopLeftHandCorner = new Panel[100];
        public Panel[] TopRightHandCorner = new Panel[100];
        public Panel[] BottomLeftHandCorner = new Panel[100];
        public Panel[] BottomRightHandCorner = new Panel[100];
        public Panel[] TopCentre = new Panel[100];
        public Panel[] RightCentre = new Panel[100];
        public Panel[] BottomCentre = new Panel[100];
        public Panel[] LeftCentre = new Panel[100];
        public Panel[] Selection = new Panel[100];
        public Control[] SelectedItems = new Control[100];
        public bool IsClicked = false, SIsPressed = false, CIsPressed = false, IsBeingDragged;
        public int selectcount = 0, InitialX, InitialY;

        public Form1()
        {
            InitializeComponent();
            MoveLogics();
            //SelectLogics();
            KeyPreview = true;
            KeyDown += MoveUsingArrowKeys;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void MoveLogics()
        {
            foreach (Control control in Controls)
            {
                control.MouseUp += (sender, args) =>
                {
                    var cc = sender as Control;
                    if (cc == null) return;
                    IsBeingDragged = false;
                    for (int i = 0; i < selectcount; i++)
                        Selection[i].Visible = true;
                };

                control.MouseDown += (sender, args) =>
                {
                    if (args.Button != MouseButtons.Left) return;
                    if (!CIsPressed && control.Tag==null)
                        SelectionGUIOff();
                    SelectionGUIOn(control);
                    IsBeingDragged = true;
                    InitialX = args.X;
                    InitialY = args.Y;
                };
                control.MouseMove += (sender, args) =>
                {
                    var cc = sender as Control;
                    if (!IsBeingDragged || cc == null || control != SelectedItems[0]) return;
                    for (int i = 0; i < selectcount; i++)
                        Selection[i].Visible = false;
                    for (int i = 0; i < selectcount; i++)
                    {
                        SelectedItems[i].Top += args.Y - InitialY;
                        SelectedItems[i].Left += args.X - InitialX;
                        TopLeftHandCorner[i].Top += args.Y  - InitialY;
                        TopLeftHandCorner[i].Left += args.X  - InitialX;
                        TopRightHandCorner[i].Top += args.Y  - InitialY;
                        TopRightHandCorner[i].Left += args.X - InitialX;
                        BottomLeftHandCorner[i].Top += args.Y- InitialY;
                        BottomLeftHandCorner[i].Left += args.X - InitialX;
                        BottomRightHandCorner[i].Top += args.Y - InitialY;
                        BottomRightHandCorner[i].Left += args.X - InitialX;
                        TopCentre[i].Top += args.Y  - InitialY;
                        TopCentre[i].Left += args.X - InitialX;
                        BottomCentre[i].Top += args.Y  - InitialY;
                        BottomCentre[i].Left += args.X  - InitialX;
                        LeftCentre[i].Top += args.Y  - InitialY;
                        LeftCentre[i].Left += args.X - InitialX;
                        RightCentre[i].Top += args.Y - InitialY;
                        RightCentre[i].Left += args.X - InitialX;
                        Selection[i].Top += args.Y - InitialY;
                        Selection[i].Left += args.X - InitialX;
                    }
                };
            }
        }

        
        private void MoveUsingArrowKeys(object sender, KeyEventArgs args)
        {
            for(int c=0;c<selectcount;c++)
            {
                for (int i = 0; i < selectcount; i++)
                    Selection[i].Visible = false;
                switch (args.KeyCode)
                {
                    case Keys.Up:
                        SelectedItems[c].Top -= 1;
                        TopLeftHandCorner[c].Top -= 1;
                        TopRightHandCorner[c].Top -= 1;
                        BottomLeftHandCorner[c].Top -= 1;
                        BottomRightHandCorner[c].Top -= 1;
                        TopCentre[c].Top -= 1;
                        BottomCentre[c].Top -= 1;
                        LeftCentre[c].Top -= 1;
                        RightCentre[c].Top -= 1;
                        Selection[c].Top -= 1;
                        break;
                    case Keys.Left:
                        SelectedItems[c].Left -= 1;
                        TopLeftHandCorner[c].Left -= 1;
                        TopRightHandCorner[c].Left -= 1;
                        BottomLeftHandCorner[c].Left -= 1;
                        BottomRightHandCorner[c].Left -= 1;
                        TopCentre[c].Left -= 1;
                        BottomCentre[c].Left -= 1;
                        LeftCentre[c].Left -= 1;
                        RightCentre[c].Left -= 1;
                        Selection[c].Left -= 1;
                        break;
                    case Keys.Down:
                        SelectedItems[c].Top += 1;
                        TopLeftHandCorner[c].Top += 1;
                        TopRightHandCorner[c].Top += 1;
                        BottomLeftHandCorner[c].Top += 1;
                        BottomRightHandCorner[c].Top += 1;
                        TopCentre[c].Top += 1;
                        BottomCentre[c].Top += 1;
                        LeftCentre[c].Top += 1;
                        RightCentre[c].Top += 1;
                        Selection[c].Top += 1;
                        break;
                    case Keys.Right:
                        SelectedItems[c].Left += 1;
                        TopLeftHandCorner[c].Left += 1;
                        TopRightHandCorner[c].Left += 1;
                        BottomLeftHandCorner[c].Left += 1;
                        BottomRightHandCorner[c].Left += 1;
                        TopCentre[c].Left += 1;
                        BottomCentre[c].Left += 1;
                        LeftCentre[c].Left += 1;
                        RightCentre[c].Left += 1;
                        Selection[c].Left += 1;
                        break;
                }
                InitialX = SelectedItems[c].Left;
                InitialX = SelectedItems[c].Top;
            }
            for (int i = 0; i < selectcount; i++)
                Selection[i].Visible = true;
        }
        /*
        private void SelectLogics()
        {
            TopLeftHandCorner.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            TopLeftHandCorner.MouseMove += (sender, args) => ResizingLogics("Top Left", TopLeftHandCorner.Tag, args);
            TopLeftHandCorner.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)TopLeftHandCorner.Tag).Location.X - 1, ((Control)TopLeftHandCorner.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)TopLeftHandCorner.Tag).Width + 2, ((Control)TopLeftHandCorner.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            TopRightHandCorner.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            TopRightHandCorner.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)TopRightHandCorner.Tag).Location.X - 1, ((Control)TopRightHandCorner.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)TopRightHandCorner.Tag).Width + 2, ((Control)TopRightHandCorner.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            TopRightHandCorner.MouseMove += (sender, args) => ResizingLogics("Top Right", TopRightHandCorner.Tag, args);
            BottomLeftHandCorner.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            BottomLeftHandCorner.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)BottomLeftHandCorner.Tag).Location.X - 1, ((Control)BottomLeftHandCorner.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)BottomLeftHandCorner.Tag).Width + 2, ((Control)BottomLeftHandCorner.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            BottomLeftHandCorner.MouseMove += (sender, args) => ResizingLogics("Bottom Left", BottomLeftHandCorner.Tag, args);
            BottomRightHandCorner.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            BottomRightHandCorner.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)BottomRightHandCorner.Tag).Location.X - 1, ((Control)BottomRightHandCorner.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)BottomRightHandCorner.Tag).Width + 2, ((Control)BottomRightHandCorner.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            BottomRightHandCorner.MouseMove += (sender, args) => ResizingLogics("Bottom Right", BottomRightHandCorner.Tag, args);
            TopCentre.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            TopCentre.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)TopCentre.Tag).Location.X - 1, ((Control)TopCentre.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)TopCentre.Tag).Width + 2, ((Control)TopCentre.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            TopCentre.MouseMove += (sender, args) => ResizingLogics("Top Centre", TopCentre.Tag, args);
            BottomCentre.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            BottomCentre.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)BottomCentre.Tag).Location.X - 1, ((Control)BottomCentre.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)BottomCentre.Tag).Width + 2, ((Control)BottomCentre.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            BottomCentre.MouseMove += (sender, args) => ResizingLogics("Bottom Centre", BottomCentre.Tag, args);
            RightCentre.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            RightCentre.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)RightCentre.Tag).Location.X - 1, ((Control)RightCentre.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)RightCentre.Tag).Width + 2, ((Control)RightCentre.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            RightCentre.MouseMove += (sender, args) => ResizingLogics("Right Centre", RightCentre.Tag, args);
            LeftCentre.MouseDown += (sender, args) => { IsClicked = true; Controls.Remove(Selection); };
            LeftCentre.MouseUp += (sender, args) => {
                IsClicked = false;
                Controls.Add(Selection);
                Selection.Location = new Point(((Control)LeftCentre.Tag).Location.X - 1, ((Control)LeftCentre.Tag).Location.Y - 1);
                Selection.Size = new Size(((Control)LeftCentre.Tag).Width + 2, ((Control)LeftCentre.Tag).Height + 2);
                Selection.BringToFront();
                ((Control)TopLeftHandCorner.Tag).BringToFront();
            };
            LeftCentre.MouseMove += (sender, args) => ResizingLogics("Left Centre", LeftCentre.Tag, args);

        }
        
        private void ResizingLogics(string Position, object control, MouseEventArgs args)
        {
            if (!IsClicked) return;
            Control control_ = (Control)control;
            switch (Position)
            {
                case "Bottom Right":
                    if (!SIsPressed)
                    {
                        control_.Width += args.X;
                        control_.Height += args.Y;
                        BottomLeftHandCorner.Top += args.Y;
                        TopRightHandCorner.Left += args.X;
                        BottomRightHandCorner.Left += args.X;
                        BottomRightHandCorner.Top += args.Y;
                        BottomCentre.Top += args.Y;
                        BottomCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                        RightCentre.Left += args.X;
                        RightCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        LeftCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        TopCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    }
                    else
                    {
                        double temp = args.X > args.Y ? args.X : args.Y, ratio = control_.Height / control_.Width;
                        int argsX = (int)(ratio > 1 ? temp : temp * control_.Width / control_.Height);
                        int argsY = (int)(ratio > 1 ? temp * ratio : temp);
                        control_.Width += argsX;
                        control_.Height += argsY;
                        BottomLeftHandCorner.Top += argsY;
                        TopRightHandCorner.Left += argsX;
                        BottomRightHandCorner.Left += argsX;
                        BottomRightHandCorner.Top += argsY;
                        BottomCentre.Top += argsY;
                        BottomCentre.Left = control_.Left + (control_.Width + argsX - 10) / 2;
                        RightCentre.Left += argsX;
                        RightCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        LeftCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        TopCentre.Left = control_.Left + (control_.Width + argsX - 10) / 2;
                    }
                    break;
                case "Bottom Left":
                    if (!SIsPressed)
                    {
                        control_.Width -= args.X;
                        control_.Left += args.X;
                        control_.Height += args.Y;
                        TopLeftHandCorner.Left += args.X;
                        BottomLeftHandCorner.Top += args.Y;
                        BottomLeftHandCorner.Left += args.X;
                        BottomRightHandCorner.Top += args.Y;
                        BottomCentre.Top += args.Y;
                        BottomCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                        LeftCentre.Left += args.X;
                        LeftCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        TopCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                        RightCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                    }
                    else
                    {
                        double temp = Math.Abs(args.X) < Math.Abs(args.Y) ? Math.Abs(args.X) : Math.Abs(args.Y), ratio = control_.Height / control_.Width;
                        int argsX = (int)(ratio > 1 ? temp : temp * control_.Width / control_.Height);
                        int argsY = (int)(ratio > 1 ? temp * ratio : temp);
                        control_.Width -= args.Y > 0 ? -argsX : argsX;
                        control_.Left += args.Y > 0 ? -argsX : argsX;
                        control_.Height += args.Y > 0 ? argsY : -argsY;
                        TopLeftHandCorner.Left += args.Y > 0 ? -argsX : argsX;
                        BottomLeftHandCorner.Top += args.Y > 0 ? argsY : -argsY;
                        BottomLeftHandCorner.Left += args.Y > 0 ? -argsX : argsX;
                        BottomRightHandCorner.Top += args.Y > 0 ? argsY : -argsY;
                        BottomCentre.Top += args.Y > 0 ? argsY : -argsY;
                        BottomCentre.Left = control_.Left + (control_.Width - argsX - 10) / 2;
                        LeftCentre.Left += args.Y > 0 ? -argsX : argsX;
                        LeftCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        TopCentre.Left = control_.Left + (control_.Width - argsX - 10) / 2;
                        RightCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                    }
                    break;
                case "Top Left":
                    if (!SIsPressed)
                    {
                        control_.Width -= args.X;
                        control_.Height -= args.Y;
                        control_.Top += args.Y;
                        control_.Left += args.X;
                        TopLeftHandCorner.Left += args.X;
                        TopLeftHandCorner.Top += args.Y;
                        TopRightHandCorner.Top += args.Y;
                        BottomLeftHandCorner.Left += args.X;
                        LeftCentre.Left += args.X;
                        LeftCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        TopCentre.Top += args.Y;
                        TopCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                        RightCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        BottomCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    }
                    else
                    {
                        double temp = args.X > args.Y ? args.X : args.Y, ratio = control_.Height / control_.Width;
                        int argsX = (int)(ratio > 1 ? temp : temp * control_.Width / control_.Height);
                        int argsY = (int)(ratio > 1 ? temp * ratio : temp);
                        control_.Width -= argsX;
                        control_.Height -= argsY;
                        control_.Top += argsY;
                        control_.Left += argsX;
                        TopLeftHandCorner.Left += argsX;
                        TopLeftHandCorner.Top += argsY;
                        TopRightHandCorner.Top += argsY;
                        BottomLeftHandCorner.Left += argsX;
                        LeftCentre.Left += argsX;
                        LeftCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        TopCentre.Top += argsY;
                        TopCentre.Left = control_.Left + (control_.Width + argsX - 10) / 2;
                        RightCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        BottomCentre.Left = control_.Left + (control_.Width + argsX - 10) / 2;
                    }
                    break;
                case "Top Right":
                    if (!SIsPressed)
                    {
                        control_.Width += args.X;
                        control_.Height -= args.Y;
                        control_.Top += args.Y;
                        TopRightHandCorner.Left += args.X;
                        TopRightHandCorner.Top += args.Y;
                        TopLeftHandCorner.Top += args.Y;
                        BottomRightHandCorner.Left += args.X;
                        TopCentre.Top += args.Y;
                        TopCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                        RightCentre.Left += args.X;
                        RightCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        LeftCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                        BottomCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    }
                    else
                    {
                        double temp = Math.Abs(args.X) < Math.Abs(args.Y) ? Math.Abs(args.X) : Math.Abs(args.Y), ratio = control_.Height / control_.Width;
                        int argsX = (int)(ratio > 1 ? temp : temp * control_.Width / control_.Height);
                        int argsY = (int)(ratio > 1 ? temp * ratio : temp);
                        control_.Width += args.X > 0 ? argsX : -argsX;
                        control_.Height -= args.X > 0 ? -argsY : argsY;
                        control_.Top += args.X > 0 ? -argsY : argsY;
                        TopRightHandCorner.Left += args.X > 0 ? argsX : -argsX;
                        TopRightHandCorner.Top += args.X > 0 ? -argsY : argsY;
                        TopLeftHandCorner.Top += args.X > 0 ? -argsY : argsY;
                        BottomRightHandCorner.Left += args.X > 0 ? argsX : -argsX;
                        TopCentre.Top += args.X > 0 ? -argsY : argsY;
                        TopCentre.Left = control_.Left + (control_.Width + argsX - 10) / 2;
                        RightCentre.Left += args.X > 0 ? argsX : -argsX;
                        RightCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        LeftCentre.Top = control_.Top + (control_.Height + argsY - 10) / 2;
                        BottomCentre.Left = control_.Left + (control_.Width + argsX - 10) / 2;
                    }
                    break;
                case "Bottom Centre":
                    control_.Height += args.Y;
                    BottomCentre.Top += args.Y;
                    BottomLeftHandCorner.Top += args.Y;
                    BottomRightHandCorner.Top += args.Y;
                    RightCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                    LeftCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                    break;
                case "Top Centre":
                    control_.Height -= args.Y;
                    control_.Top += args.Y;
                    TopCentre.Top += args.Y;
                    TopLeftHandCorner.Top += args.Y;
                    TopRightHandCorner.Top += args.Y;
                    RightCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                    LeftCentre.Top = control_.Top + (control_.Height + args.Y - 10) / 2;
                    break;
                case "Right Centre":
                    control_.Width += args.X;
                    RightCentre.Left += args.X;
                    TopRightHandCorner.Left += args.X;
                    BottomRightHandCorner.Left += args.X;
                    BottomCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    TopCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    break;
                case "Left Centre":
                    control_.Width -= args.X;
                    control_.Left += args.X;
                    LeftCentre.Left += args.X;
                    TopLeftHandCorner.Left += args.X;
                    BottomLeftHandCorner.Left += args.X;
                    BottomCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    TopCentre.Left = control_.Left + (control_.Width + args.X - 10) / 2;
                    break;
            }
        }
        */
        public void SelectionGUIOn(Control control)
        {
            TopLeftHandCorner[selectcount] = new Panel();
            Controls.Add(TopLeftHandCorner[selectcount]);
            TopLeftHandCorner[selectcount].Location = new Point(control.Location.X - 5, control.Location.Y - 5);
            TopLeftHandCorner[selectcount].Size = new Size(10, 10);
            TopLeftHandCorner[selectcount].BackColor = Color.Aqua;
            TopLeftHandCorner[selectcount].Tag = control;
            TopRightHandCorner[selectcount] = new Panel();
            Controls.Add(TopRightHandCorner[selectcount]);
            TopRightHandCorner[selectcount].Location = new Point(control.Location.X + control.Width - 5, control.Location.Y - 5);
            TopRightHandCorner[selectcount].Size = new Size(10, 10);
            TopRightHandCorner[selectcount].BackColor = Color.Aqua;
            TopRightHandCorner[selectcount].Tag = control;
            BottomLeftHandCorner[selectcount] = new Panel();
            Controls.Add(BottomLeftHandCorner[selectcount]);
            BottomLeftHandCorner[selectcount].Location = new Point(control.Location.X - 5, control.Location.Y + control.Height - 5);
            BottomLeftHandCorner[selectcount].Size = new Size(10, 10);
            BottomLeftHandCorner[selectcount].BackColor = Color.Aqua;
            BottomLeftHandCorner[selectcount].Tag = control;
            BottomRightHandCorner[selectcount] = new Panel();
            Controls.Add(BottomRightHandCorner[selectcount]);
            BottomRightHandCorner[selectcount].Location = new Point(control.Location.X + control.Width - 5, control.Location.Y + control.Height - 5);
            BottomRightHandCorner[selectcount].Size = new Size(10, 10);
            BottomRightHandCorner[selectcount].BackColor = Color.Aqua;
            BottomRightHandCorner[selectcount].Tag = control;
            TopCentre[selectcount] = new Panel();
            Controls.Add(TopCentre[selectcount]);
            TopCentre[selectcount].Location = new Point(control.Left + (control.Width-10)/2, control.Top-5);
            TopCentre[selectcount].Size = new Size(10, 10);
            TopCentre[selectcount].BackColor = Color.Aqua;
            TopCentre[selectcount].Tag = control;
            LeftCentre[selectcount] = new Panel();
            Controls.Add(LeftCentre[selectcount]);
            LeftCentre[selectcount].Location = new Point(control.Left - 5, control.Location.Y + (control.Height - 10)/2);
            LeftCentre[selectcount].Size = new Size(10, 10);
            LeftCentre[selectcount].BackColor = Color.Aqua;
            LeftCentre[selectcount].Tag = control;
            BottomCentre[selectcount] = new Panel();
            Controls.Add(BottomCentre[selectcount]);
            BottomCentre[selectcount].Location = new Point(control.Left + (control.Width - 10) / 2, control.Top + control.Height - 5);
            BottomCentre[selectcount].Size = new Size(10, 10);
            BottomCentre[selectcount].BackColor = Color.Aqua;
            BottomCentre[selectcount].Tag = control;
            RightCentre[selectcount] = new Panel();
            Controls.Add(RightCentre[selectcount]);
            RightCentre[selectcount].Location = new Point(control.Left + control.Width - 5, control.Location.Y + (control.Height - 10) / 2);
            RightCentre[selectcount].Size = new Size(10, 10);
            RightCentre[selectcount].BackColor = Color.Aqua;
            RightCentre[selectcount].Tag = control;
            Selection[selectcount] = new Panel();
            Selection[selectcount].Visible = false;
            Controls.Add(Selection[selectcount]);
            Selection[selectcount].Location = new Point(control.Location.X - 2, control.Location.Y - 2);
            Selection[selectcount].Size = new Size(control.Width + 4, control.Height + 4);
            Selection[selectcount].BackColor = Color.Cyan;
            TopLeftHandCorner[selectcount].BringToFront();
            TopRightHandCorner[selectcount].BringToFront();
            BottomLeftHandCorner[selectcount].BringToFront();
            BottomRightHandCorner[selectcount].BringToFront();
            TopCentre[selectcount].BringToFront();
            LeftCentre[selectcount].BringToFront();
            RightCentre[selectcount].BringToFront();
            BottomCentre[selectcount].BringToFront();
            Selection[selectcount].BringToFront();
            control.BringToFront();
            Selection[selectcount].Visible = true;
            control.Tag = "Selected";
            SelectedItems[selectcount] = control;
            selectcount++;
        }

        public void SelectionGUIOff()
        {
            for (int i = 0; i < selectcount; i++)
            {
                Controls.Remove(Selection[i]);
                Controls.Remove(TopLeftHandCorner[i]);
                Controls.Remove(TopRightHandCorner[i]);
                Controls.Remove(BottomLeftHandCorner[i]);
                Controls.Remove(BottomRightHandCorner[i]);
                Controls.Remove(TopCentre[i]);
                Controls.Remove(LeftCentre[i]);
                Controls.Remove(BottomCentre[i]);
                Controls.Remove(RightCentre[i]);
                SelectedItems[i].Tag = null;
                SelectedItems[i] = null;
            }
            selectcount = 0;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            SelectionGUIOff();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
                SIsPressed = false;
            if (e.KeyCode == Keys.C)
                CIsPressed = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
                SIsPressed = true;
            if (e.KeyCode == Keys.C)
                CIsPressed = true;
        }
    }
}
