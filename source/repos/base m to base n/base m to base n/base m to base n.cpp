#include "pch.h"
#include <iostream>
#include <string>
#include <cmath>


using namespace std;
int m, n, num2, num3, result1, num1fract_1, num1int_1;
double num1, result,result2;

int nrofdigits(int z)
{
	int l = 0;
	for (; z != 0; l++) z /= 10;
	return l;
}

int nrofdecimals(double z)
{
	int l = 0;
	for (;abs(z-floor(z))>1e-8; l++) z *= 10;

	return l;
}

int invert(int z)
{
	while (z != 0)
	{
		int y = z % 10;
		result1 = 10 * result1 + y;
		z /= 10;
		if (y >= m)
		{
			cout << "ERROR";
			exit(0);
		}
	}
	return result1;
}

int bmtob10_integer(int m, int num1int)
{
	num1int_1 = num1int;
	for (int k = 0; k < nrofdigits(num1int_1); k++)
	{
		num3 = num1int % 10;
		num2 += num3 * pow(m, k);
		num1int /= 10;
		if (num3 >= m)
		{
			cout << "ERROR";
			exit(0);
		}
	}
	return num2;
}

double bmtob10_fraction(int m, double num1fract)
{
	num1fract *= pow(10, nrofdecimals(num1fract));
	int num1fract1 = num1fract;
	num1fract_1 = num1fract1;
	num1fract1 = invert(num1fract1);
	for (int k = 1; k <= nrofdigits(num1fract_1); k++)
	{
		num3 = num1fract1 % 10;
		result2 += num3 * pow(m, -k);
		num1fract1 /= 10;
	}
	return result2;
}

double bmtob10(int m, double num1)
{
	double num1int, num1fract;
	num1fract = modf(num1, &num1int);
	return bmtob10_integer(m, num1int) + bmtob10_fraction(m, num1fract);
}

string binaryinteger(int num)
{
	int bin;
	string string;
	while (num > 0)
	{
		bin = num % n;
		string = to_string(bin) + string;
		num /= n;
	}
	return string;
}
string binaryfraction(double fr)
{
	double result1;
	int remainder;
	string string1;
	while (fr > 0)
	{
		result1 = fr * n;
		remainder = floor(result1);
		fr = result1 - remainder;
		string1 = string1 + to_string(remainder);
		if (string1.length() > 100) break;
	} 
	return string1;
}

void b10tobn(int n, double num1)
{
	if (num1 < 0)
		cout << "-";
	if ((-1 < num1) && (1 > num1))
		cout << "0";
	if (ceil(num1) == num1)
	{
		if (num1 >= 0)
		{
			cout << (binaryinteger(num1));
		}

		else
		{
			num1 = -num1;
			cout << binaryinteger(num1);
		}
	}
	else
	{
		int intpart;
		double frpart;

		if (num1 >= 0)
		{
			intpart = floor(num1);
			frpart = num1 - intpart;
			cout << binaryinteger(intpart) << "." << binaryfraction(frpart);
		}

		else
		{
			intpart = ceil(num1);
			frpart = intpart - num1;
			intpart = -intpart;
			cout << binaryinteger(intpart) << "." << binaryfraction(frpart);
		}
	}
}

int main()
{
BASE1:
	{cout << "Enter the base you want to convert the number from: ";
	cin >> m; }
	if (m > 9 || m < 2)
	{
		cout << "Error" << endl;
			goto BASE1;
	}
BASE2:
	{cout << "Enter the base you want to convert the number to: ";
	cin >> n; }
	if (n > 9 || n < 2)
	{
		cout << "Error" << endl;
		goto BASE2;
	}	cout << "Enter the number you want to convert: ";
	cin >> num1;
	cout << num1 << " (base " << m << ") to base " << n << " is ";
	num1 = bmtob10(m, num1);
	b10tobn(n, num1);
}
