﻿using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Testura.Code.Builder.Factory;
using Testura.Code.Helper;
using Testura.Code.Helper.References;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Testura.Code.Statements
{
    public class DeclerationStatement
    {
        /// <summary>
        /// Declare and assign a local variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="value">Value to assign variable</param>
        /// <param name="useVarKeyword">True if we should use var keyword, otherwise we use type name</param>
        /// <returns>The generated local declaration statement</returns>
        public LocalDeclarationStatementSyntax DeclareAndAssign<T>(string name, T value, bool useVarKeyword = true)
            where T : struct
        {
            return LocalDeclarationStatement(VariableDeclaration(IdentifierName(useVarKeyword ? "var" : typeof(T).Name))
                .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(name))
                    .WithInitializer(EqualsValueClauseFactory.GetEqualsValueClause(value).WithEqualsToken(Token(SyntaxKind.EqualsToken))))));
        }

        /// <summary>
        /// Declare and a assign a local variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="value">Value to assign variable</param>
        /// <param name="useVarKeyword">If we should created the variable with the var keyword</param>
        /// <returns>The generated local declaration statement</returns>
        public LocalDeclarationStatementSyntax DeclareAndAssign(string name, string value, bool useVarKeyword = true)
        {
            return LocalDeclarationStatement(VariableDeclaration(IdentifierName(useVarKeyword ? "var" : typeof(string).Name))
                .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(name))
                    .WithInitializer(EqualsValueClauseFactory.GetEqualsValueClause($@"""{value}""").WithEqualsToken(Token(SyntaxKind.EqualsToken))))));
        }


        /// <summary>
        /// Declare and assign a local variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="reference">Value of the variable</param>
        /// <param name="useVarKeyword">If we should created the variable with the var keyword</param>
        /// <returns>The generated local declaration statement</returns>
        public LocalDeclarationStatementSyntax DeclareAndAssign(string name, Type type, VariableReference reference, bool useVarKeyword = true)
        {
            return LocalDeclarationStatement(VariableDeclaration(IdentifierName(useVarKeyword ? "var" : type.Name))
                .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(name))
                    .WithInitializer(EqualsValueClauseFactory.GetEqualsValueClause(reference).WithEqualsToken(Token(SyntaxKind.EqualsToken))))));
        }

        /// <summary>
        /// Declare and assign a local variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="arguments">Arguments to use when creating the variable</param>
        /// <param name="useVarKeyword">If we should created the variable with the var keyword</param>
        /// <returns>The generated local declaration statement</returns>
        public LocalDeclarationStatementSyntax DeclareAndAssign(string name, Type type, ArgumentListSyntax arguments, bool useVarKeyword = true)
        {
            var typeName = NameConverters.ConvertGenericTypeName(type);
            return LocalDeclarationStatement(VariableDeclaration(IdentifierName(useVarKeyword ? "var" : typeName))
                .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(name))
                    .WithInitializer(
                        EqualsValueClause(
                            ObjectCreationExpression(IdentifierName(typeName)).WithArgumentList(arguments)
                                .WithNewKeyword(Token(SyntaxKind.NewKeyword)))))));
        }

        /// <summary>
        /// Declare and assign a local variable
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Type of the variable </param>
        /// <param name="expressionSyntax">An already created expression to assign to the variable</param>
        /// <param name="castTo">Cast the expression to this type</param>
        /// <param name="useVarKeyword">If we should create the variable with the var keyword</param>
        /// <returns>The generated local decleration statement</returns>
        public LocalDeclarationStatementSyntax CreateLocal(string name, Type type, ExpressionSyntax expressionSyntax, Type castTo = null, bool useVarKeyword = true)
        {
            if (castTo != null && castTo != typeof(void))
            {
                expressionSyntax = CastExpression(IdentifierName(castTo.FullName), expressionSyntax);
            }

            return LocalDeclarationStatement(VariableDeclaration(IdentifierName(useVarKeyword ? "var" : type.Name))
                .WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(name))
                    .WithInitializer(EqualsValueClause(expressionSyntax)))));
        }

        /// <summary>
        /// Assign a variable
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="arguments">Árguments in the class constructor</param>
        /// <returns>The generated assign decleration statement</returns>
        public ExpressionStatementSyntax Assign(string name, Type type, ArgumentListSyntax arguments)
        {
            var typeName = NameConverters.ConvertGenericTypeName(type);
            return ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(name),
                    ObjectCreationExpression(IdentifierName(typeName)).WithArgumentList(arguments).WithNewKeyword(Token(SyntaxKind.NewKeyword))));
        }

        /// <summary>
        /// Assign a variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="expressionSyntax">The expression syntax </param>
        /// <param name="castTo">If we should do a cast while assign the variable</param>
        /// <returns>The generated assign decleration syntax</returns>
        public ExpressionStatementSyntax Assign(string name, ExpressionSyntax expressionSyntax, Type castTo = null)
        {
            if (castTo != null && castTo != typeof(void))
            {
                expressionSyntax = CastExpression(IdentifierName(castTo.Name), expressionSyntax);
            }

            return
                ExpressionStatement(
                    AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(name),
                    expressionSyntax));
        }
    }
}