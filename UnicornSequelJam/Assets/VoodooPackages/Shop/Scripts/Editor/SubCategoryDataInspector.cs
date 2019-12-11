using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CustomEditor(typeof(SubCategoryData), true)]
    public class SubCategoryDataInspector : Editor
    {
        private SubCategoryData subCategoryData;
        private string[] paymentTypes;
        private string[] paymentTypeNames;
        public static int selectedIndex;
        
        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            subCategoryData = (SubCategoryData)target;

            Type baseType = typeof(Payment);
            List<Type> childTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => baseType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();

            paymentTypes = childTypes.Select(x => x.AssemblyQualifiedName).ToArray();
            paymentTypeNames = childTypes.Select(x => x.ToString().Split('.').Last()).ToArray();
        }
        
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            subCategoryData.payment = (Payment)EditorGUILayout.ObjectField("Payment", subCategoryData.payment, typeof(Payment), false);
            
            EditorGUI.BeginChangeCheck();
                
            if (subCategoryData.payment != null)
                selectedIndex = paymentTypes.ToList().FindIndex(x => Type.GetType(x) == subCategoryData.payment.GetType());
            else
            {
                Type paymentType = typeof(PaymentCurrency);
                subCategoryData.CreatePaymentInstance(paymentType);
                selectedIndex = paymentTypes.ToList().FindIndex(x => Type.GetType(x) == subCategoryData.payment.GetType());
            }
            
            selectedIndex = EditorGUILayout.Popup("Payment Type", selectedIndex, paymentTypeNames);
            
            if (EditorGUI.EndChangeCheck())
            {
                Type paymentType = Type.GetType(paymentTypes[selectedIndex]);
                subCategoryData.CreatePaymentInstance(paymentType);
            }
            
            EditorGUILayout.BeginVertical("box");
            if (subCategoryData.payment is PaymentCurrency paymentCurrency)
            {
                DrawPaymentCurrency(paymentCurrency);
            }
            else if (subCategoryData.payment is PaymentChestRoom paymentChestRoom)
                DrawPaymentChestRoom(paymentChestRoom);
            else if (subCategoryData.payment is PaymentComingSoon paymentComingSoon)
                DrawPaymentComingSoon(paymentComingSoon);
            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(subCategoryData);
        }
        
        private void DrawPaymentCurrency(PaymentCurrency _paymentCurrency)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            {
                _paymentCurrency.currency = (Currency)EditorGUILayout.ObjectField(GUIContent.none, _paymentCurrency.currency, typeof(Currency), false, GUILayout.Width(200));
                EditorGUILayout.LabelField(new GUIContent("Cost"), GUILayout.Width(80));
                _paymentCurrency.cost = EditorGUILayout.DoubleField(GUIContent.none, _paymentCurrency.cost);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                _paymentCurrency.lockedOnNotPurchasable = EditorGUILayout.Toggle( GUIContent.none, _paymentCurrency.lockedOnNotPurchasable, GUILayout.Width(12));
                EditorGUILayout.LabelField(new GUIContent("Locked On Not Purchasable"), GUILayout.Width(184));
                EditorGUILayout.LabelField(new GUIContent("Cost Multiplier"), GUILayout.Width(80));
                _paymentCurrency.costMultiplier = EditorGUILayout.FloatField(GUIContent.none, _paymentCurrency.costMultiplier);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                _paymentCurrency.NumberOfPurchaseDone = EditorGUILayout.IntField(new GUIContent("Number of Purchase Done :"),_paymentCurrency.NumberOfPurchaseDone);
            }
            EditorGUILayout.EndHorizontal();
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_paymentCurrency);
        }

        private void DrawPaymentChestRoom(PaymentChestRoom _paymentChestRoom)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("Message"), GUILayout.Width(70));
                _paymentChestRoom.message = EditorGUILayout.TextField(GUIContent.none, _paymentChestRoom.message);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("Requirement"), GUILayout.Width(70));
                _paymentChestRoom.requirement = EditorGUILayout.DoubleField(GUIContent.none, _paymentChestRoom.requirement);
            }
            EditorGUILayout.EndHorizontal();
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_paymentChestRoom);
        }

        private void DrawPaymentComingSoon(PaymentComingSoon _paymentComingSoon)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(new GUIContent("Message"), GUILayout.Width(70));
                _paymentComingSoon.message = EditorGUILayout.TextField(GUIContent.none, _paymentComingSoon.message);
            }
            EditorGUILayout.EndHorizontal();
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_paymentComingSoon);
        }
    }
}