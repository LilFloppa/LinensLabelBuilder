# Linens Label Builder

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
![image](https://user-images.githubusercontent.com/44492725/127853348-35d62542-fd59-4623-a584-487d518ac73d.png)

## Click "Добавить" button
### Added labels will appear in the list on the right. You can also delete unwanted labels by selecting them and clicking "Удалить выбранные" button.
![image](https://user-images.githubusercontent.com/44492725/127854989-02e59091-7639-4461-8db4-8314fe9a73c9.png)

## After adding all desired labels click "Печать" button to print labels
### Only four labels can fit on one A4 sheet. If number of labels is more then four, additional A4 sheets will be used.
![image](https://user-images.githubusercontent.com/44492725/127855043-8d59d8e1-f55f-4e10-8149-8663b1be7ace.png)

## Format parameters window
* ### Font size ("Размер шрифта")
* ### Margin ("Размеры отступов") - margin for each label in resulting A4 sheet
* ### Offset for sizes of linens ("Отступ для размеров белья") - offset for numeric values of linen's dimentions
![image](https://user-images.githubusercontent.com/44492725/127857505-d9e2dab3-8d0b-47ea-8a0b-465cf9355034.png)



