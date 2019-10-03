﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Testura.Code.Builders.BuildMembers
{
    public class MethodMember : IBuildMember
    {
        private readonly MethodDeclarationSyntax[] _methodDeclarationSyntaxs;

        public MethodMember(MethodDeclarationSyntax[] methodDeclarationSyntaxs)
        {
            _methodDeclarationSyntaxs = methodDeclarationSyntaxs;
        }

        public SyntaxList<MemberDeclarationSyntax> AddMember(SyntaxList<MemberDeclarationSyntax> members)
        {
            foreach (var methodDeclarationSyntax in _methodDeclarationSyntaxs)
            {
                members = members.Add(methodDeclarationSyntax);
            }

            return members;
        }
    }
}