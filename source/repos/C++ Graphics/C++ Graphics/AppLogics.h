#pragma once
#include <SFML/Graphics.hpp>
#include <cstdlib>
#include <vector>
#include <chrono>
#include <thread>
#include <iostream>
#include "Application.h"

template <class T>
using vec = std::vector<T>;
using vec2 = sf::Vector2<float>;

const float Width = 8.f, Step = 5.f, Spacing = 1.f, XOffset = 100, YOffset = 1000;
const int n = 200;

struct rec : sf::RectangleShape
{
public:
	void SetPosition(vec2 vec)
	{
		Position = vec;
		setPosition(vec2(vec.x, vec.y - getSize().y));
	}
	vec2 GetPosition()
	{
		return Position;
	}
	void SetHeight(int height)
	{
		Height = height;
		setSize(vec2(Width, Step * height));
		SetPosition(Position);
	}
	int GetHeight()
	{
		return Height;
	}
private:
	vec2				Position;
	float				Height;
};

struct AppLogics
{
	int swaps, comparisons;
	void GenerateSound(int);
	void Verify();
	void Delay(int);
	void SetUp();
	void Refresh();
	void Shuffle();
	void SelectionSort();
	void InsertionSort();
	void BubbleSort();
	void MergeSort(int left = 0, int right = n - 1);
	void QuickSort(int left = 0, int right = n - 1);
	int partition(int, int);
	void HeapSort();
	void heapify(int, int);
	void BucketSort();
	void Swap(int, int, int);
};