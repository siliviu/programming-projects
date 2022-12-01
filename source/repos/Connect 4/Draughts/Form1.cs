using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;


namespace Connect4 {
	public enum Colour { Yellow, Red, None };
	public partial class Form1 : Form {
		public int GridHeight = 6, GridWidth = 7, WinNumber = 4, PieceSize = 90, GridSize = 100, Offx = 20, Offy = 20, Border = 0;
		public int[] dx = { 0, 1, 1, 1, 0, -1, -1, -1 }, dy = { 1, 1, 0, -1, -1, -1, 0, 1 };
		public Tile[,] Grid = new Tile[1 + 15, 1 + 15];
		public Piece Last;
		public List<Piece> WhitePieces = new List<Piece>(), BlackPieces = new List<Piece>();
		public Colour Turn = Colour.Yellow, Player;
		public byte[] Buffer;
		public Socket ServerSocket, ClientSocket;
		public Label lw, lb;
		public Form1(Colour c = Colour.None, string ip = "127.0.0.1") {
			InitializeComponent();
			Player = c;
			InitialiseGame();
			InitialiseGUI();
			if (Player != Colour.None)
				InitialiseNetworking(ip);
		}
		private void Form1_Load(object sender, EventArgs e) {
		}
		public void InitialiseGame() {
			for (int i = 1; i <= GridHeight; ++i)
				for (int j = 1; j <= GridWidth; ++j) {
					Grid[i, j] = new Tile(i, j, GridSize, Offx, Offy, Border, this);
					Controls.Add(Grid[i, j]);
				}
		}
		public void InitialiseGUI() {
			lb = new Label() {
				AutoSize = true,
				Location = new Point(Offx + GridWidth * (GridSize + Border) + 40, Offy + 10)
			};
			lw = new Label() {
				AutoSize = true,
				Location = new Point(Offx + GridWidth * (GridSize + Border) + 40, -10 - lb.Size.Height + Offy + GridHeight * (GridSize + Border))
			};
			Controls.Add(lw);
			Controls.Add(lb);
			UpdateGUI();
		}
		public void UpdateGUI() {
			lw.Text = "Yellow Pieces : " + WhitePieces.Count();
			lb.Text = "Red Pieces : " + BlackPieces.Count();
		}
		public void InitialiseNetworking(string ip) {
			void ReceiveCallback(IAsyncResult AR) {
				try {
					int received = ClientSocket.EndReceive(AR);
					if (received == 0)
						return;
					Invoke((Action)delegate {
						if (Buffer[0] == 3)
							MessageBox.Show("Match found");
						else if (Buffer[0] == 2)
							EndTurn();
						else if (Buffer[0] == 1) {
							int c = Buffer[1];
							HandleMove(c);
						}
					});
					ClientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, null);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
			if (Player == Colour.Yellow) {
				try {
					ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 6012));
					ServerSocket.Listen(1);
					ServerSocket.BeginAccept(AcceptCallback, null);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
				void AcceptCallback(IAsyncResult AR) {
					try {
						ClientSocket = ServerSocket.EndAccept(AR);
						Buffer = new byte[ClientSocket.ReceiveBufferSize];
						ClientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, null);
						ServerSocket.BeginAccept(AcceptCallback, null);
						SendRequest(3);
					}
					catch (Exception ex) {
						MessageBox.Show(ex.Message);
					}
				}
			}
			else {
				ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				try {
					var endPoint = new IPEndPoint(IPAddress.Parse(ip), 6012);
					ClientSocket.BeginConnect(endPoint, ConnectCallback, null);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
				void ConnectCallback(IAsyncResult AR) {
					try {
						ClientSocket.EndConnect(AR);
						Buffer = new byte[ClientSocket.ReceiveBufferSize];
						ClientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, null);
						SendRequest(3);
					}
					catch (Exception ex) {
						MessageBox.Show(ex.Message);
					}
				}
			}
		}
		public void SendRequest(int type, int c = 0) {
			if (Player == Colour.None) return;
			byte[] Message = null;
			if (type == 3)
				Message = new byte[] { 3 };
			else if (type == 2)
				Message = new byte[] { 2 };
			else {
				Message = new byte[] { 1, (byte)c };
			}
			ClientSocket.BeginSend(Message, 0, Message.Length, SocketFlags.None, SendCallback, null);
			void SendCallback(IAsyncResult AR) {
				try {
					ClientSocket.EndSend(AR);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
		}
		public void HandleMove(int column) {
			for (int i = GridHeight; i >= 1; --i)
				if (Grid[i, column].HasPiece == null) {
					Piece NewPiece = new Piece(Turn, PieceSize, this);
					Controls.Add(NewPiece);
					NewPiece.BringToFront();
					if (Turn == Colour.Yellow)
						WhitePieces.Add(NewPiece);
					else
						BlackPieces.Add(NewPiece);
					NewPiece.Place(Grid[i, column]);
					Last = NewPiece;
					UpdateGUI();
					return;
				}
		}
		public bool CheckForLine() {
			int y = Last.OnTile.Row, x = Last.OnTile.Column;
			for (int k = 0; k < 4; ++k) {
				int[] a = { WinNumber - 1, WinNumber - 1 };
				for (int kk = 0; kk < 2; ++kk) {
					int ak = k + 4 * kk;
					for (int i = 1; i < WinNumber; ++i) {
						int yy = y + i * dy[ak], xx = x + i * dx[ak];
						if (yy < 1 || xx < 1 || yy > GridHeight || xx > GridWidth || Grid[yy, xx].HasPiece == null || Grid[yy, xx].HasPiece.PieceColour != Turn) {
							a[kk] = i - 1;
							break;
						}
					}
				}
				if (a[0] + a[1] >= WinNumber - 1) {
					for (int i = 0; i < WinNumber; ++i) {
						int yy = y + (i - a[1]) * dy[k], xx = x + (i - a[1]) * dx[k];
						Grid[yy, xx].BackColor = Color.Green;
					}
					return true;
				}
			}
			return false;
		}
		public void EndTurn() {
			if (CheckForLine())
				EndGame(Turn);
			else if (WhitePieces.Count() + BlackPieces.Count() == GridHeight * GridWidth)
				EndGame(Colour.None);
			Turn = (Colour)((int)Turn ^ 1);
		}
		public void EndGame(Colour Winner) {
			MessageBox.Show("Game over! " + Winner.ToString() + " won the game.");
			Turn = Colour.None;
		}
	}

	public class Piece : PictureBox {
		public Form1 f;
		public Colour PieceColour;
		public Tile OnTile;
		public void Place(Tile tile) {
			Parent = tile;
			Location = new Point((tile.Size.Width - Size.Width) / 2, (tile.Size.Height - Size.Height) / 2);
			OnTile = tile;
			MouseDown += tile.ClickHandler;
			tile.HasPiece = this;
		}
		public Piece(Colour c, int size, Form1 form) {
			Size = new Size(size, size);
			PieceColour = c;
			BackColor = Color.Transparent;
			BackgroundImageLayout = ImageLayout.Stretch;
			BackgroundImage = PieceColour == Colour.Yellow ? Properties.Resources.white : Properties.Resources.black;
			f = form;
		}
	}
	public class Tile : PictureBox {
		public Form1 f;
		public int Row, Column;
		public Piece HasPiece;
		public Tile(int r, int c, int size, int offx, int offy, int border, Form1 form) {
			Row = r;
			Column = c;
			Size = new Size(size, size);
			Location = new Point(offx + (c - 1) * (size + border), offy + (r - 1) * (size + border));
			BackColor = (c & 1) == 1 ? Color.Navy : Color.DarkBlue;
			BackgroundImage = Properties.Resources.empty;
			BackgroundImageLayout = ImageLayout.Stretch;
			MouseDown += ClickHandler;
			f = form;
		}
		public void ClickHandler(object sender, MouseEventArgs e) {
			if (f.Turn == (Colour)((int)f.Player ^ 1) || f.Grid[1, Column].HasPiece != null) return;
			f.SendRequest(1, Column);
			f.HandleMove(Column);
			f.EndTurn();
			f.SendRequest(2);
		}
	}
}

/* 
 TO DO
- Jumped pieces need to be removed after the turn is over, not after each jump 
	-> Make it an option
 */