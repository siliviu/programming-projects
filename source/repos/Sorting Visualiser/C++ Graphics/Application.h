#pragma once
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>

class Application
{
public:
	static sf::RenderWindow _mainwindow;
	static void Start();
private:
	static void AppLoop();
	static void Initialise();
	enum AppState { Uninitialised, Initialising, Exiting, Idle, Sorting };
	static AppState _appstate;
};