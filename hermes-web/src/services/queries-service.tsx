import { useQuery } from 'react-query';
import { ArticleAPI } from '../api/ArticleAPI';
import { ArticleTemplateAPI } from '../api/ArticleTemplateAPI';
import { ForumPostApi } from '../api/ForumPostApi';
import { RoomAPI } from '../api/RoomAPI';
import { StaticAPI } from '../api/StaticAPI';
import { UserAPI } from '../api/UserAPI';

const defaultConfig = {
    staleTime: 6000
};

export function useArticlesQuery(roomID: string, archived: boolean) {
    return useQuery(['ARTICLES', roomID, archived], (key, roomID: string, archived: boolean) => ArticleAPI.query(roomID, archived).then(res => res.data), defaultConfig);
}
export function useArticleQuery(articleID: string) {
    return useQuery(['ARTICLE', articleID], (key, articleID: string) => ArticleAPI.get(articleID).then(res => res.data), defaultConfig);
}
export function useActivityFeedQuery() {
    return useQuery(['ACTIVITY_FEED'], () => ArticleAPI.getActivityFeed().then(res => res.data), defaultConfig);
}
export function useUserActivityFeedQuery(userID: string) {
    return useQuery(['ACTIVITY_FEED', userID], () => ArticleAPI.getActivityFeed(userID).then(res => res.data), defaultConfig);
}
export function useArticleTemplateQuery(articleTemplateID: string) {
    return useQuery(['ARTICLE_TEMPLATE', articleTemplateID], (key, articleTemplateID: string) => ArticleTemplateAPI.get(articleTemplateID).then(res => res.data), defaultConfig);
}
export function useArticleTemplatesQuery(archived: boolean, languageID: string|null, topicID: string|null) {
    return useQuery(['ARTICLE_TEMPLATES', archived, languageID, topicID], (key, archived: boolean, languageID: string, topicID: string|null) => ArticleTemplateAPI.query(archived, languageID, topicID).then(res => res.data), defaultConfig);
}
export function useRoomQuery(roomID: string) {
    return useQuery(['ROOM', roomID], (key, roomID: string) => RoomAPI.get(roomID).then(res => res.data), defaultConfig);
}
export function useRoomUsersQuery(roomID: string) {
    return useQuery(['ROOM_USERS', roomID], (key, roomID: string) => RoomAPI.getUsers(roomID).then(res => res.data), defaultConfig);
}
export function useRoomPendingUsersQuery(roomID: string) {
    return useQuery(['ROOM_PENDING_USERS', roomID], (key, roomID: string) => RoomAPI.getPendingUsers(roomID).then(res => res.data), defaultConfig);
}
export function useRoomsQuery(filter:string, userID:string, languageID1:string, languageID2:string) {
    return useQuery(
        ['ROOMS', filter, userID, languageID1, languageID2],
        (key, filter:string, userID:string, languageID1:string, languageID2:string) => RoomAPI.query(filter, userID, languageID1, languageID2).then(res => res.data),
        defaultConfig
    );
}
export function useLanguagesQuery() {
    return useQuery(['LANGUAGES'], () => StaticAPI.getLanguages().then(res => res.data), defaultConfig);
}
export function useTopicsQuery() {
    return useQuery(['TOPIC'], () => StaticAPI.getTopics().then(res => res.data), defaultConfig);
}
export function useUserQuery(userID: string) {
    return useQuery(['USER', userID], (key, userID: string) => UserAPI.get(userID).then(res => res.data), defaultConfig);
}
export function useUserDetailsQuery(userID: string) {
    return useQuery(['USER_DETAILS', userID], (key, userID: string) => UserAPI.getUserDetails(userID).then(res => res.data), defaultConfig);
}
export function useUsersQuery() {
    return useQuery(['USERS'], () => UserAPI.query().then(res => res.data), defaultConfig);
}
export function useRankingQuery() {
    return useQuery(['RANKING'], () => UserAPI.getRanking().then(res => res.data), defaultConfig);
}
export function useUserPostsQuery(userID: string) {
    return useQuery(['USER_POSTS', userID], (key, userID: string) => UserAPI.getUserPosts(userID).then(res => res.data), defaultConfig);
}
export function useForumQuery(forumPostID: string) {
    return useQuery(['FORUM_POST', forumPostID], (key, forumPostID: string) => ForumPostApi.getForumPost(forumPostID).then(res => res.data), defaultConfig);
}
export function useForumFeedQuery( languageID: string|null ) {
    return useQuery(['FORUM_FEED', languageID ], (key, languageID: string ) => ForumPostApi.query(languageID).then(res => res.data), defaultConfig);
}
export function useUsersComments( forumPostID: string ) {
    return useQuery(['FORUM_COMMENTS', forumPostID ], (key, forumPostID: string ) => ForumPostApi.getPostComments(forumPostID).then(res => res.data), defaultConfig);
}

