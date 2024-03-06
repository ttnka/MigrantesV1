using System;
using Microsoft.AspNetCore.Components;

namespace TorneosV2.Pages.Sistema.Mensajes
{
	public class ExitosBase : ComponentBase
	{
		[Inject]
		public NavigationManager NM { get; set; } = default!;

		protected override async Task OnInitializedAsync()
		{
			
		}
	}
}

