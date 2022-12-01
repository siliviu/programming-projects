#include "AppLogics.h"

sf::Font arial;
sf::Text Swaps, Comparisons;
int swaps = 0, comparisons = 0;
vec<rec> Bars;

void AppLogics::Delay(int ms)
{
	std::this_thread::sleep_for(std::chrono::milliseconds(ms));
}

sf::SoundBuffer buffer;
vec<sf::Int16> samples;
sf::Sound Sound;

void AppLogics::GenerateSound(int nr)
{
	int scount = 44 * 50;
	double x = 0;
	for (int i = 0; i < 0.4 * scount; i++)
	{
		double temp = (double)i / scount;
		samples.push_back(floor(5000 * ((temp < 0.2) ? (5 * temp) : (2 - 5 * temp))) * sin(x * 6.28319));
		x += floor(1000 * (nr / (double)n) + 100) / 44100;
	}
	buffer.loadFromSamples(&samples[0], samples.size(), 1, 44100);
	samples.clear();
	Sound.setBuffer(buffer);
	Sound.play();
}

void AppLogics::Verify()
{
	for (int i = 0; i < n; i++)
	{
		/**/Bars[i].setFillColor(sf::Color::Green);
		GenerateSound(i);
		Delay(4);
		Refresh();
		/**/Bars[i].setFillColor(sf::Color::White);
	}
}

void AppLogics::Swap(int x, int y, int delay)
{
	swaps++;
	int temp = Bars[x].GetHeight();
	Bars[x].SetHeight(Bars[y].GetHeight());
	Bars[y].SetHeight(temp);
	Delay(delay);
	Refresh();
}

void AppLogics::SetUp()
{
	arial.loadFromFile("arial.ttf");
	Swaps.setFont(arial);
	Comparisons.setFont(arial);
	Swaps.setCharacterSize(24);
	Comparisons.setCharacterSize(24);
	Swaps.setFillColor(sf::Color::Red);
	Comparisons.setFillColor(sf::Color::Red);
	Swaps.setPosition(vec2(40, 40));
	Comparisons.setPosition(vec2(100, 40));
	for (int i = 0; i < n; i++)
	{
		rec temp;
		temp.setFillColor(sf::Color::White);
		temp.SetHeight(i + 1);
		temp.SetPosition(vec2(XOffset + i * (Width + Spacing), YOffset));
		Bars.push_back(temp);
	}
	Refresh();
}

void AppLogics::Refresh()
{
	Application::_mainwindow.clear(sf::Color::Black);
	for (auto& rect : Bars)
		Application::_mainwindow.draw(rect);
	Swaps.setString(std::to_string(swaps));
	Comparisons.setString(std::to_string(comparisons));
	Application::_mainwindow.draw(Swaps);
	Application::_mainwindow.draw(Comparisons);
	Application::_mainwindow.display();
}

void AppLogics::Shuffle()
{
	srand((unsigned)time * time(NULL));
	vec<int> temp;
	for (int i = 0; i < n; i++)
		temp.push_back(i);
	while (!temp.empty())
	{
		int randint = rand() % temp.size();
		Bars[n - temp.size()].SetHeight(temp[randint] + 1);
		temp.erase(temp.begin() + randint);
		Delay(5);
		Refresh();
	}
}

void AppLogics::SelectionSort()
{
	for (int i = 0; i < n - 1; i++)
	{
		for (int j = i + 1; j < n; j++)
		{
			/**/Bars[j].setFillColor(sf::Color::Red);
			GenerateSound(j);
			if (++comparisons && Bars[j].GetHeight() < Bars[i].GetHeight())
				Swap(i, j, 1);
			/**/Bars[j].setFillColor(sf::Color::White);
		}
		/**/if (i != n - 2)	Bars[i].setFillColor(sf::Color::Green);
		/**/if (i) Bars[i - 1].setFillColor(sf::Color::White);
	}
}

void AppLogics::InsertionSort()
{
	for (int i = 1; i < n; i++)
	{
		int j = i - 1;
		/**/Bars[i].setFillColor(sf::Color::Green);
		while (++comparisons && j >= 0 && Bars[j + 1].GetHeight() < Bars[j].GetHeight())
		{
			/**/Bars[j].setFillColor(sf::Color::Red);
			GenerateSound(j);
			Swap(j, j-- + 1, 2);
			/**/Bars[j + 1].setFillColor(sf::Color::White);
		}
		/**/Bars[i].setFillColor(sf::Color::White);
	}
}

void AppLogics::BubbleSort()
{
	for (int i = 0; i < n; i++)
	{
		/**/Bars[n - i].setFillColor(sf::Color::Green);
		for (int j = 0; j < n - i - 1; j++)
		{
			/**/Bars[j].setFillColor(sf::Color::Red);
			GenerateSound(j);
			if (++comparisons && Bars[j].GetHeight() > Bars[j + 1].GetHeight())
				Swap(j, j + 1, 0);
			/**/Bars[j].setFillColor(sf::Color::White);
		}
		/**/Bars[n - i].setFillColor(sf::Color::White);
	}
}

void AppLogics::MergeSort(int left, int right)
{
	if (left >= right) return;
	int m = (left + right) / 2;
	/**/Bars[m].setFillColor(sf::Color::Green);
	MergeSort(left, m);
	MergeSort(m + 1, right);
	int i = left, j = m + 1;
	vec<int> temp;
	while (i <= m && j <= right)
		if (++comparisons && Bars[i].GetHeight() < Bars[j].GetHeight())
			temp.push_back(Bars[i++].GetHeight());
		else
			temp.push_back(Bars[j++].GetHeight());
	while (i <= m)
		temp.push_back(Bars[i++].GetHeight());
	while (j <= right)
		temp.push_back(Bars[j++].GetHeight());
	for (int x = 0; x < temp.size(); x++)
	{
		swaps++;
		Bars[left + x].SetHeight(temp[x]);
		/**/if (Bars[left + x].getFillColor() != sf::Color::Green)	Bars[left + x].setFillColor(sf::Color::Red);
		GenerateSound(j);
		Delay(5);
		Refresh();
		/**/if (Bars[left + x].getFillColor() != sf::Color::Green) Bars[left + x].setFillColor(sf::Color::White);
	}
	/**/Bars[m].setFillColor(sf::Color::White);
}

void AppLogics::QuickSort(int left, int right)
{
	if (left >= right) return;
	int pivot = partition(left, right);
	Bars[(left + right) / 2].setFillColor(sf::Color::White);
	Bars[pivot].setFillColor(sf::Color::Cyan);
	QuickSort(left, pivot);
	QuickSort(pivot + 1, right);
	Bars[pivot].setFillColor(sf::Color::White);
}

int AppLogics::partition(int left, int right)
{
	Bars[(left + right) / 2].setFillColor(sf::Color::Green);
	int pivot = Bars[(left + right) / 2].GetHeight();
	int i = left - 1, j = right + 1;
	while (i < j)
	{
		do { i++; } while (++comparisons && Bars[i].GetHeight() < pivot);
		do { j--; } while (++comparisons && Bars[j].GetHeight() > pivot);
		if (i >= j) return j;
		/**/Bars[i].setFillColor(sf::Color::Red);
		/**/Bars[j].setFillColor(sf::Color::Red);
		GenerateSound(i);
		GenerateSound(j);
		Swap(i, j, 10);
		/**/Bars[i].setFillColor(sf::Color::White);
		/**/Bars[j].setFillColor(sf::Color::White);
	}
}

void AppLogics::BucketSort()
{
	vec<int> Buckets[n / 10 + 1];
	for (int i = 0; i < n; i++)
		Buckets[Bars[i].GetHeight() / 10].push_back(Bars[i].GetHeight());
	int current = 0;
	for (int i = 0; i < n / 10 + 1; i++)
		while (Buckets[i].size())
		{
			swaps++;
			Bars[current++].SetHeight(Buckets[i].front());
			Buckets[i].erase(Buckets[i].begin());
			/**/Bars[current].setFillColor(sf::Color::Red);
			GenerateSound(current);
			Delay(25);
			Refresh();
			/**/Bars[current].setFillColor(sf::Color::White);
		}
	InsertionSort();
}

void AppLogics::heapify(int n, int i)
{
	int largest = i;
	int l = 2 * i + 1;
	int r = 2 * i + 2;
	if (++comparisons && l < n && Bars[l].GetHeight() > Bars[largest].GetHeight())
		largest = l;
	if (++comparisons && r < n && Bars[r].GetHeight() > Bars[largest].GetHeight())
		largest = r;
	if (largest == i) return;
	/**/Bars[largest].setFillColor(sf::Color::Red);
	GenerateSound(largest);
	Swap(i, largest, 10);
	/**/Bars[largest].setFillColor(sf::Color::White);
	heapify(n, largest);
}

void AppLogics::HeapSort()
{
	for (int i = n / 2 - 1; i >= 0; i--)
		heapify(n, i);
	for (int i = n - 1; i >= 0; i--)
	{
		/**/Bars[i].setFillColor(sf::Color::Green);
		Swap(0, i, 10);
		heapify(i, 0);
		/**/Bars[i].setFillColor(sf::Color::White);
	}
}
