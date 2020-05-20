### Stacking гиперказуалка

Changelog  относительно окончания стрима
----------------------------------------
#### Изменено:
- Логика работы GameManager'a (Assets/Scripts/GameManager.cs)
	- В Awake устанавливается `Application.targetFrameRate = 120` (для айпадов)
        Unity по умолчанию запускает приложение в 30fps, и если не знать того что нужно менять данный параметр, можно очень хорошо оптимизировать приложение, но визуально оно будет все равно работать неплавно. Например, очень сильно это видно при скроллинге в UI
    - Изменены состояния и их обработка
    - Добавлены события, которые вызываются на определенные состояния и действия
        Вызов происходит с использованием null-conditional оператора. Следующие две строки кода призваны пояснить его использование. Они полностью эквивалентны
        ```csharp
		a?.Method();
		if(a != null) a.Method();
        ```
    - При помощи колбэка OnValidate задается значения `bounds.extends` по `y` бесконечность (чтобы случайно не сбросить)
    - Добавлены аттрибуты (такие штуки в квадратных скобках  перед объявлением поля) для отрображения под. информации в редакторе

#### Добавлено:
- Цвета (Assets/Scripts/Colors.cs)
- Градиент в бэкграунде
    Реализовано при помощи VertexColor шейдера (Assets/Shaders/VertexColor.shader) и скрипта, устанавливающего цвета для vertex'ов (Assets/Scripts/Gradient.cs)
- Snap-to-grid (extension-метод в Assets/Scripts/Extensions.cs)
- Отдельный менеджер UI (Assets/Scripts/UIManager.cs)
- Тактильная отдача (Assets/Scripts/Feedback.cs)
    Реализовно при помощи [TapticPlugin](https://github.com/asus4/unity-taptic-plugin) для ios и `Handheld.Vibrate()` для всего остального

-------------
### Рекомендовано к прочтению и использованию
- Любая книга за авторством Эндрю Троелсена. Например, **"Язык программирования C# 7 и платформы .NET и .NET Core, 8-е издание"**
- [Официальная документация Microsoft по C#](https://docs.microsoft.com/ru-ru/dotnet/csharp/)
- [Официальная документация Unity](https://docs.unity3d.com/Manual/index.html)
- [Доска в трелло где написано как не надо делать](https://trello.com/b/Z6cDRyis/good-coding-practices-in-unity)
- [Планин для работы с TapticEngine](https://github.com/asus4/unity-taptic-plugin), поддерживает установку через upm
- [Плагин для работы с экраном IphoneX и IPad Pro и старше](https://bitbucket.org/p12tic/iossafeareasplugin/src). [Подробная статья на хабре](https://habr.com/ru/company/pixonic/blog/351184/)