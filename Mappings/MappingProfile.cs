using AutoMapper;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<TechStack, TechStackResponseDto>();
			CreateMap<TechStackCreateDto, TechStack>();
			CreateMap<TechStackUpdateDto, TechStack>();

			CreateMap<ProjectCategory, ProjectCategoryResponseDto>();
			CreateMap<ProjectCategoryCreateDto, ProjectCategory>();
			CreateMap<ProjectCategoryUpdateDto, ProjectCategory>();

			CreateMap<ProjectDomain, ProjectDomainResponseDto>();
			CreateMap<ProjectDomainCreateDto, ProjectDomain>();
			CreateMap<ProjectDomainUpdateDto, ProjectDomain>();
		}
	}
}
