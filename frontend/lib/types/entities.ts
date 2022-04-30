export interface Comment {
	id: string;
	content: string;
	postId: string;
	userId: string;
	repliesToId: string;
	creationTimeString: string;
	post: Post;
	user: User;
	repliesTo: Comment;
	replies: Comment[];
}

export interface Post {
	id: string;
	title: string;
	content: string;
	creationTimeString: string;
	tags: string[];
	userId: string;
	user: User;
	comments: Comment[];
}

export interface User {
	avatar: string;
	bio: string;
	discordProfile: string;
	instagramProfile: string;
	followedTags: string[];
	posts: Post[];
	friendships: Friendship[];
	userName: string;
	email: string;
}

export interface Friendship {
	id: string;
	users: User[];
}
