Сформулировано 22.11.2019
- Ввести функциональность сдвига баннеров при наложении из версии OKO-02_Test-2. Сделано. Позже перевести из Update в Coroutine.
- Использовать новую версию MapBox.
- Ввести глобальный коэфициент масштабирования - для всех объектов сцены, перемещений, и границ действия камеры - 
возможность динамически регулировать масштаб всей сцены.
- Использовать новую версию OculusVR UI.
- Меню настроек и экранный пульт управления.
- Управление контроллером X-Box.
- Выбор аэропорта и перезагрузка сцены.
- Найти карты SIP/STAR.

В версии Unity 2019.2.13 работает нормально. 2019.2.15 и 2019.3.0 - ошибка при сборке проекта Gradle. 2019.2.14 не проверялась.
Остаемся пока в версии 2019.2.13

Готовим для Android ArCore

Настойки для Android-версии.
1. Bulid Settings. Установить Platform - Android.
2. Project Settings/Player.
Company Name: AviAReaL
Product Name: OKO
Other Settings
Rendering
Auto Graphics APIs: Off
Graphics APIs: удалить Vulkan
Identification
Package Name: com.AviAReaL.OKO
Minimum API Level: Android 7.0
Target API Level: Android 7.0
Configuration
API Compatibility Level: .NET 4.x
3. Project Settings/Quality.
Shadows
Shadows: Hard Shadows Only
Shadow Resolution: High Resolution
Shadow Distance: 30000
3. В сцене.
Boss/S Record
Write log: Off
Write Web Data: Off