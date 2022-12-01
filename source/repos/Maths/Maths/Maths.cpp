#include "pch.h"
#include <iostream>

using namespace std;

long double f (long double a, long double b)
{
	return a * (b + 1);
}

int main()
{
	long double S[1000], R;
	cin >> S[0] >> S[1];
	R = 1 / S[0] + 1 / S[1];
	for (int i = 2; i < 1000; i++)
	{
		S[i] = f(S[i - 2], S[i - 1]);
		R += 1 / S[i];
	}
	cout << "From 1 to inf: " << R << endl << "From 3 to inf: " << R - 1 / S[0] - 1 / S[1];
}