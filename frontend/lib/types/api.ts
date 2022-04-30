export interface CreateCommentDto {
	content: string;
	repliesToId: string;
}

export interface EditCommentDto {
	content: string;
	commentId: string;
}

export interface LoginUserDto {
	email: string;
	password: string;
}

export interface RegisterUserDto {
	email: string;
	username: string;
	password: string;
	avatar: string;
	discordProfile: string;
	instagramProfile: string;
	bio: string;
	followedTags: string[];
}

export interface CreatePostDto {
	title: string;
	content: string;
	tags: string[];
}

export interface EditPostDto {
	content: string;
}

export interface Response {
	success: boolean;
	errors: ApplicationError[];
}

export interface LoginUserResponse extends Response {
	token: string;
}

export interface ApplicationError {
	code: string;
	description: string;
}

export interface PaginatedResponse<T> extends Response {
	success: boolean;
	pages: number;
	currentPage: number;
	data: T;
	errors: ApplicationError[];
}