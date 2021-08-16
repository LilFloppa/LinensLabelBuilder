<img src="https://user-images.githubusercontent.com/44492725/127859023-05c935d0-1c01-41c3-bc1d-ba13920b454f.png" alt="[logo]" width="48"/> Linens Label Builder
=======================

## Description
Small application that can bulid labels for linens. Labels contain name of product, sizes, cloth name and price. Optionally, can contain sizes for bedsheets on elastic.

## Installation
 1. Download source code
 2. Build source code in Visual Studio
 
## Requirements
  * .NET 5.0
  
## Used techonologies
  * WPF
  * [ReactiveUI](https://www.reactiveui.net/)
  * [FluentValidaton](https://fluentvalidation.net/)

# Usage
## Enter the parameters you want
![image](https://user-images.githubusercontent.com/44492725/129508131-8681d74c-8837-4e36-ad70-60f6f96ebebe.png)

## Click "Добавить" button
### Added labels will appear in the list on the right. You can also delete unwanted labels by selecting them and clicking "Удалить выбранные" button.
![image](https://user-images.githubusercontent.com/44492725/129508198-0dce38c8-a4cd-4edf-b738-569001493eb7.png)

## After adding all desired labels click "Печать" button to print labels
### Only four labels can fit on one A4 sheet. If number of labels is more then four, additional A4 sheets will be used.
![image](https://user-images.githubusercontent.com/44492725/129508231-8977bdc5-5d3f-43b0-a13a-6b6fba78c500.png)

## Format parameters window
* ### Font size ("Размер шрифта")
* ### Margin ("Размеры отступов") - margin for each label in resulting A4 sheet
* ### Offset for sizes of linens ("Отступ для размеров белья") - offset for numeric values of linen's dimentions
![image](https://user-images.githubusercontent.com/44492725/127857505-d9e2dab3-8d0b-47ea-8a0b-465cf9355034.png)



