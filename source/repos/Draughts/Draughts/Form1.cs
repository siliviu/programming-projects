using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;


namespace Draughts {
	public enum Colour { White, Black, Light, Dark, None };
	public enum State { Idle, Active };
	public enum Type { Man, King };
	public partial class Form1 : Form {
		public int GridLength = 8, PieceRows = 3, PieceSize = 75, GridSize = 100, Offx = 20, Offy = 20, Border = 0;
		public bool FlyingKing = false, JumpBackwards = false, MandatoryCapture = false, Forced = false, AvailableJump = false;
		public int[] dx = { -1, 1, 1, -1 }, dy = { -1, -1, 1, 1 };
		public Tile[,] Grid = new Tile[1 + 15, 1 + 15	];
		public List<Piece> WhitePieces = new List<Piece>(), BlackPieces = new List<Piece>();
		public Piece ActivePiece = null;
		public List<Tile> ActiveTiles = new List<Tile>();
		public Colour Turn = Colour.White, Player;
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
			for (int i = 1; i <= GridLength; ++i)
				for (int j = 1; j <= GridLength; ++j) {
					Grid[i, j] = new Tile(i, j, (Colour)(2 + (i + j) % 2), GridSize, Offx, Offy, Border, this);
					Controls.Add(Grid[i, j]);
				}
			for (int i = 1; i <= PieceRows; ++i)
				for (int j = 1; j <= GridLength; ++j)
					if ((i + j) % 2 == 1) {
						BlackPieces.Add(new Piece(Colour.Black, PieceSize, this));
						Controls.Add(BlackPieces.Last());
						BlackPieces.Last().Place(Grid[i, j]);
						BlackPieces.Last().BringToFront();
					}
			for (int i = GridLength - PieceRows + 1; i <= GridLength; ++i)
				for (int j = 1; j <= GridLength; ++j)
					if ((i + j) % 2 == 1) {
						WhitePieces.Add(new Piece(Colour.White, PieceSize, this));
						Controls.Add(WhitePieces.Last());
						WhitePieces.Last().Place(Grid[i, j]);
						WhitePieces.Last().BringToFront();
					}

		}
		public void InitialiseGUI() {
			lb = new Label() {
				AutoSize = true,
				Location = new Point(Offx + GridLength * (GridSize + Border) + 40, Offy + 10)
			};
			lw = new Label() {
				AutoSize = true,
				Location = new Point(Offx + GridLength * (GridSize + Border) + 40, -10 - lb.Size.Height + Offy + GridLength * (GridSize + Border))
			};
			Controls.Add(lw);
			Controls.Add(lb);
			UpdateGUI();
		}
		public void UpdateGUI() {
			lw.Text = "White Pieces : " + WhitePieces.Count();
			lb.Text = "Black Pieces : " + BlackPieces.Count();
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
							int y0 = Buffer[1], x0 = Buffer[2], y1 = Buffer[3], x1 = Buffer[4];
							HandleMove(y0, x0, y1, x1);
						}
					});
					ClientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, null);
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
			if (Player == Colour.White) {
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
		public void SendRequest(int type, int y0 = 0, int x0 = 0, int y1 = 0, int x1 = 0) {
			if (Player == Colour.None) return;
			byte[] Message = null;
			if (type == 3)
				Message = new byte[] { 3 };
			else if (type == 2)
				Message = new byte[] { 2 };
			else {
				Message = new byte[] { 1, (byte)y0, (byte)x0, (byte)y1, (byte)x1 };
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
		public List<Tile> CheckForMoves(Piece piece, bool AllowWalks) {
			int x0 = piece.OnTile.Column, y0 = piece.OnTile.Row;
			Colour Colour = piece.PieceColour;
			List<Tile> Moves = new List<Tile>();
			bool IsKing = piece.Type == Type.King;
			void Do(int start, bool cond) {
				for (int i = start, j = 0; j < 2; ++i, ++j) {
					int dj = dx[i], di = dy[i], yj = -1, xj = -1;
					for (int k = 1; k <= (IsKing && FlyingKing ? GridLength : 1); ++k) {
						int y = y0 + di * k, x = x0 + dj * k;
						if (y > GridLength || x > GridLength || y <= 0 || x <= 0)
							break;
						Tile CurGrid = Grid[y, x];
						Piece CurPiece = CurGrid.HasPiece;
						if (cond && CurPiece == null)
							Moves.Add(CurGrid);
						if (CurPiece != null) {
							if (CurPiece.PieceColour != Colour) {
								yj = y;
								xj = x;
							}
							break;
						}
					}
					if (yj == -1) continue;
					for (int k = 1; k <= (IsKing && FlyingKing ? GridLength : 1); ++k) {
						int y = yj + di * k, x = xj + dj * k;
						if (y > GridLength || x > GridLength || y <= 0 || x <= 0)
							break;
						Tile CurGrid = Grid[y, x];
						Piece CurPiece = CurGrid.HasPiece;
						if (CurPiece == null)
							Moves.Add(CurGrid);
						if (CurPiece != null)
							break;
					}
				}
			}
			Do(2 * (int)Colour, AllowWalks);
			if (JumpBackwards || IsKing)
				Do(2 * ((int)Colour ^ 1), AllowWalks && IsKing);
			return Moves;
		}

		public void HandleMove(int ystart, int xstart, int yfinish, int xfinish) {
			Piece CurrentPiece = Grid[ystart, xstart].HasPiece;
			int Step = Math.Abs(ystart - yfinish);
			CurrentPiece.Place(Grid[yfinish, xfinish]);
			if (Step >= 2) {
				int di = (yfinish - ystart) / Step, dj = (xfinish - xstart) / Step;
				Tile CapturedGrid = null;
				for (int i = 1; i < Step; ++i) {
					int y = ystart + di * i, x = xstart + dj * i;
					Tile CurrentGrid = Grid[y, x];
					if (CurrentGrid.HasPiece != null && CurrentGrid.HasPiece.PieceColour != CurrentPiece.PieceColour) {
						CapturedGrid = CurrentGrid;
						break;
					}
				}
				if (CapturedGrid != null)
					CapturedGrid.HasPiece.Remove();
				else
					EndTurn();
			}
			else
				EndTurn();
		}
		public bool CheckForMoves(Colour Colour, bool walks) {
			foreach (Piece piece in Colour == Colour.White ? WhitePieces : BlackPieces)
				if (CheckForMoves(piece, walks).Count != 0)
					return true;
			return false;
		}
		public void EndTurn() {
			Forced = false;
			if (ActivePiece != null)
				ActivePiece.Toggle();
			if (!CheckForMoves((Colour)((int)Turn ^ 1), true))
				EndGame(Turn);
			AvailableJump = CheckForMoves((Colour)((int)Turn ^ 1), false);
			Turn = (Colour)((int)Turn ^ 1);
		}
		public void EndGame(Colour Winner) {
			MessageBox.Show("Game over! " + Winner.ToString() + " won the game.");
			Turn = Colour.None;
		}
	}

	public class Piece : PictureBox {
		public Form1 f;
		public Type Type = Type.Man;
		public State State = State.Idle;
		public Colour PieceColour;
		public Tile OnTile;
		public void Place(Tile tile) {
			if (OnTile != null)
				OnTile.HasPiece = null;
			Parent = tile;
			Location = new Point((tile.Size.Width - Size.Width) / 2, (tile.Size.Height - Size.Height) / 2);
			if ((PieceColour == Colour.White && tile.Row == 1) || (PieceColour == Colour.Black && tile.Row == f.GridLength)) {
				Type = Type.King;
				BackgroundImage = PieceColour == Colour.White ? Properties.Resources.whiteking : Properties.Resources.blackking;
			}
			OnTile = tile;
			tile.HasPiece = this;
		}
		public void Remove() {
			OnTile.HasPiece = null;
			if (PieceColour == Colour.White)
				f.WhitePieces.Remove(this);
			else
				f.BlackPieces.Remove(this);
			f.UpdateGUI();
			if (f.WhitePieces.Count == 0)
				f.EndGame(Colour.Black);
			else if (f.BlackPieces.Count == 0)
				f.EndGame(Colour.White);
			Dispose();
		}
		public Piece(Colour c, int size, Form1 form) {
			Size = new Size(size, size);
			PieceColour = c;
			BackColor = Color.Transparent;
			BackgroundImageLayout = ImageLayout.Stretch;
			BackgroundImage = PieceColour == Colour.White ? Properties.Resources.whiteman : Properties.Resources.blackman;
			MouseDown += ClickHandler;
			f = form;
		}
		public void Toggle() {
			if (State == State.Active) {
				f.ActivePiece = null;
				State = State.Idle;
				BackColor = Color.Transparent;

			}
			else {
				f.ActivePiece = this;
				State = State.Active;
				BackColor = Color.Yellow;
			}
		}
		public void ClickHandler(object sender, MouseEventArgs e) {
			if (f.Turn == (Colour)((int)f.Player ^ 1) || f.Turn != PieceColour || f.Forced) return;
			if (f.ActivePiece != null)
				f.ActivePiece.Toggle();
			foreach (Tile tile in f.ActiveTiles)
				tile.Toggle();
			f.ActiveTiles.Clear();
			Toggle();
			List<Tile> PossibleMoves = f.CheckForMoves(this, !(f.MandatoryCapture && f.AvailableJump));
			if (PossibleMoves.Count == 0)
				return;
			foreach (Tile Tile in PossibleMoves)
				Tile.Toggle();
		}
	}
	public class Tile : PictureBox {
		public Form1 f;
		public int Row, Column;
		public State State = State.Idle;
		public Colour TileColour;
		public Piece HasPiece;
		public void Toggle() {
			if (State == State.Active) {
				State = State.Idle;
				BackColor = TileColour == Colour.Light ? Color.PowderBlue : Color.DarkBlue;
			}
			else {
				f.ActiveTiles.Add(this);
				State = State.Active;
				BackColor = Color.Green;
			}
		}
		public Tile(int r, int c, Colour col, int size, int offx, int offy, int border, Form1 form) {
			Row = r;
			Column = c;
			TileColour = col;
			Size = new Size(size, size);
			Location = new Point(offx + (c - 1) * (size + border), offy + (r - 1) * (size + border));
			BackColor = col == Colour.Light ? Color.PowderBlue : Color.DarkBlue;
			MouseDown += ClickHandler;
			f = form;
		}
		private void ClickHandler(object sender, MouseEventArgs e) {
			if (f.Turn == (Colour)((int)f.Player ^ 1) || State == State.Idle) return;
			Colour CurrentColour = f.ActivePiece.PieceColour;
			foreach (Tile tile in f.ActiveTiles)
				tile.Toggle();
			f.ActiveTiles.Clear();
			f.SendRequest(1, f.ActivePiece.OnTile.Row, f.ActivePiece.OnTile.Column, Row, Column);
			f.HandleMove(f.ActivePiece.OnTile.Row, f.ActivePiece.OnTile.Column, Row, Column);
			if (f.Turn == CurrentColour) {
				List<Tile> PossibleMoves = f.CheckForMoves(f.ActivePiece, false);
				if (PossibleMoves.Count == 0) {
					f.EndTurn();
					f.SendRequest(2);
				}
				else {
					foreach (Tile Tile in PossibleMoves)
						Tile.Toggle();
					f.Forced = true;
				}
			}
		}
	}
}

/* 
 TO DO
- Jumped pieces need to be removed after the turn is over, not after each jump 
	-> Make it an option
 */