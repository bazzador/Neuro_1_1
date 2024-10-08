﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;

class LinearClassifier
{
    public double[] coefficients;
    public double bias;
    private double learningRate;
    private int maxEpochs;

    public LinearClassifier(double[] initialCoefficients, double initialBias = -1.2, double learningRate = 0.15, int maxEpochs = 1000)
    {
        this.coefficients = initialCoefficients;
        this.learningRate = learningRate;
        this.bias = initialBias;
        this.maxEpochs = maxEpochs;
    }

    public void Train(List<DataPoint> dataSet)
    {
        int iteration = 1;
        bool hasMisclassified = true;
        int epoch = 0;

        while (hasMisclassified)
        {
            epoch++;
            hasMisclassified = false;
            foreach (var dataPoint in dataSet)
            {
                double output = ComputeOutput(dataPoint.x1, dataPoint.x2);

                int actualLabel = dataPoint.label;
                int predictedLabel = output >= 0 ? 1 : -1;

                if (predictedLabel != actualLabel)
                {
                    coefficients[0] += learningRate * (actualLabel - predictedLabel) * dataPoint.x1;
                    coefficients[1] += learningRate * (actualLabel - predictedLabel) * dataPoint.x2;
                    bias += learningRate * (actualLabel - predictedLabel);

                    hasMisclassified = true;
                }

                Console.WriteLine($"Ітерація {iteration}: ваги: w1 = {coefficients[0]:f3}, w2 = {coefficients[1]:f3}, bias = {bias:f3}");
                iteration++;
            }

            if (epoch >= maxEpochs)
            {
                break;
            }
        }

        Console.WriteLine($"Фінальні ваги: w1 = {coefficients[0]}, w2 = {coefficients[1]}, bias = {bias}");
    }

    public double ComputeOutput(double x1, double x2)
    {
        return coefficients[0] * x1 + coefficients[1] * x2 + bias;
    }
}

class DataPoint
{
    public double x1 { get; set; }
    public double x2 { get; set; }
    public int label { get; set; }

    public DataPoint(double x1, double x2, int label)
    {
        this.x1 = x1;
        this.x2 = x2;
        this.label = label;
    }

}

class Program
{
    public static double net(double x1, double x2, double[] coefficients, double bias)
    {
        return coefficients[0] * x1 + coefficients[1] * x2 + bias;
    }
    public static int Predict(double x1, double x2, double[] coefficients, double bias)
    {
        double net_ = net(x1, x2, coefficients, bias);
        return net_ >= 0 ? 1 : -1;
    }
    static List<DataPoint> CreateDataSet(List<DataPoint> existingDataSet)
    {
        List<DataPoint> dataSet;
        if (existingDataSet == null)
        {
            dataSet = new List<DataPoint>();
        }
        else
        {
            dataSet = existingDataSet;
        }
        LinearClassifier classifier = new LinearClassifier(initialCoefficients: new double[] { 0.8, 0.4 }, initialBias: -1.2, learningRate: 0.15, maxEpochs: 1000);

        while (true)
        {
            Console.Write("Введiть x: ");
            string inputX1 = Console.ReadLine();
            if (inputX1.ToLower() == "stop")
                break;
            else if (inputX1.ToLower() == "check")
            {
                Console.WriteLine(Predict(Convert.ToDouble(Console.ReadLine()), Convert.ToDouble(Console.ReadLine()), new double[] { classifier.coefficients[0], classifier.coefficients[1] }, classifier.bias));
                continue;
            }
            Console.Write("Введiть y: ");
            string inputX2 = Console.ReadLine();
            if (inputX2.ToLower() == "stop")
                break;
            else if (inputX2.ToLower() == "check")
            {
                Console.WriteLine(Predict(Convert.ToDouble(Console.ReadLine()), Convert.ToDouble(Console.ReadLine()), new double[] { classifier.coefficients[0], classifier.coefficients[1] }, classifier.bias));
                continue;
            }

            Console.Write("Введiть -1 або 1: ");
            string inputLabel = Console.ReadLine();
            if (inputLabel.ToLower() == "stop")
                break;
            else if (inputLabel.ToLower() == "check")
            {
                Console.WriteLine(Predict(Convert.ToDouble(Console.ReadLine()), Convert.ToDouble(Console.ReadLine()), new double[] { classifier.coefficients[0], classifier.coefficients[1] }, classifier.bias));
                continue;
            }
            if (double.TryParse(inputX1, out double x1) && double.TryParse(inputX2, out double x2) && int.TryParse(inputLabel, out int label))
            {
                dataSet.Add(new DataPoint(x1, x2, label));
                Console.WriteLine("Точка додана!");
            }
            else
            {
                Console.WriteLine("Неправильне введення");
            }
        }

        Console.WriteLine("Введення завершене.");
        classifier.Train(dataSet);

        double slope = -classifier.coefficients[0] / classifier.coefficients[1];
        double intercept = -classifier.bias / classifier.coefficients[1];

        Console.WriteLine($"y = {slope}x + {intercept}".Replace(',', '.'));
        Console.WriteLine("Введіть 'continue', щоб продовжити додавання точок:");
        string cont = Console.ReadLine();
        if (cont == "continue")
            CreateDataSet(dataSet);
        Console.ReadKey();
        return dataSet;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Введіть 'stop', щоб завершити введення:");
        Console.WriteLine("Введіть 'check', щоб перевірити до якої групи прив'язана точка");
        List<DataPoint> dataSet = CreateDataSet(null);
    }
}