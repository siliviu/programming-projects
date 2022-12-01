#include "pch.h"
#include <iostream>


using namespace std;
char I = 'I';
char V = 'V';
char X = 'X';
char L = 'L';
char C = 'C';
char D = 'D';
char M = 'M';

void basic(int n, char a, char b, char c)
{
	if (n <= 3)
	{
		for (; n > 0; n--)
			cout << a;
	}
	if (n == 4) cout << a << b;
	if (n >= 5 && n < 9)
	{
		cout << b;
		n -= 5;
		{
			for (; n > 0; n--)
				cout << a;
		}
	}
	if (n == 9) cout << a << c;
}

int main()
{
	cout << "Enter the arabic number you wish to convert to roman numerals: ";
	int y, y0, y1, y2, y3;
	cin >> y;
	if (y > 3999 || y < 1) cout << "Conversion not possible";
	else
	{
		y3 = (y % 10000 - y % 1000) / 1000;
		basic(y3, M, I, I);
		y2 = (y % 1000 - y % 100) / 100; 
		basic(y2, C, D, M);
		y1 = (y % 100 - y % 10) / 10;
		basic(y1, X, L, C);
		y0 = (y % 10 - y % 1) / 1;
		basic(y0, I, V, X);
	}
}