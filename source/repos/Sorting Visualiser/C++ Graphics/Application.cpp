#include "Application.h"
#include "AppLogics.h"


Application::AppState Application::_appstate = Uninitialised;
sf::RenderWindow Application::_mainwindow;
AppLogics app;

void Application::Start()
{
	if (_appstate != Uninitialised) return;
	_mainwindow.create(sf::VideoMode(1920, 1080), "Sorting", sf::Style::Fullscreen);
	_appstate = Initialising;
	while (_appstate != Exiting)
		AppLoop();
	_mainwindow.close();
}

void Application::Initialise()
{
	app.SetUp();
	_appstate = Idle;
}

void Application::AppLoop()
{
	sf::Event currentEvent;
	while (_mainwindow.pollEvent(currentEvent))
	{
		if (currentEvent.type == sf::Event::Closed)
			_appstate = Exiting;
		switch (_appstate)
		{
		case Initialising:
			Initialise();
			break;
		case Idle:
			if (currentEvent.type == sf::Event::KeyPressed && sf::Keyboard::isKeyPressed(sf::Keyboard::S))
			{
				_appstate = Sorting;
				app.Shuffle();
				app.swaps = 0;
				app.comparisons = 0;
				_appstate = Idle;
			}
			else if (currentEvent.type == sf::Event::KeyPressed && sf::Keyboard::isKeyPressed(sf::Keyboard::F))
			{
				_appstate = Sorting;
				app.QuickSort();
				app.Verify();
				_appstate = Idle;
			}
			app.Refresh();
			break;
		case Sorting:
			break;
		}
	}
}