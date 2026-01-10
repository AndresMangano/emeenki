# YouTube Video Upload Feature - Implementation Summary

## Overview
This feature allows users to upload YouTube videos to the application. The video title and captions are extracted and used to create a "Video Article" that can be translated by users.

## What Was Implemented

### Backend (C# / .NET)

#### 1. Database Schema Changes
- **File**: `hermes-api/Shell/Scripts/0020_ADD_VIDEO_SUPPORT.sql`
- Added `IsVideo`, `VideoURL`, and `TopicID` columns to `Query_ArticleTemplate` and `Query_Article` tables
- Added `StartTimeMs` and `EndTimeMs` columns to `Query_ArticleTemplateSentence` and `Query_Sentence` tables for video caption synchronization

#### 2. YouTube Service (yt-dlp Integration)
- **File**: `hermes-api/Core/Services/YouTubeService.cs`
- Implements YouTube video metadata and caption extraction using yt-dlp
- Features:
  - Extracts video ID from various YouTube URL formats
  - Downloads video info (title, thumbnail, language)
  - Downloads captions (prioritizes manual captions over auto-generated)
  - Parses VTT caption files into structured data with timestamps
  - Automatic cleanup of temporary files

#### 3. New Commands and Events
- **Command**: `ArticleTemplateUploadVideoCommand.cs` - Accepts YouTube URL, language, and topic
- **Event**: `ArticleTemplateVideoUploadedEvent.cs` - Contains video data with timestamped captions
- **Worker Event**: `hermes-worker/Core/Model/Events/ArticleTemplate/ArticleTemplateVideoUploadedEvent.cs` - Handles database updates

#### 4. Updated Files
- `ArticleTemplateCommands.cs` - Added `ExecuteAsync` method for video uploads
- `ArticleTemplateEvents.cs` - Added handler for video-uploaded event
- `ArticleTemplatesController.cs` - Added `/api/articleTemplate/upload-video` endpoint
- `ArticleTemplateDTO.cs` - Updated to support video fields and sentences with timestamps
- `ArticleTemplatesDTO.cs` - Added `IsVideo` and `VideoURL` fields
- `ArticleTemplateQuery.cs` - Updated to fetch sentences with timestamps
- Repository files - Added methods for inserting video data with timestamps

### Frontend (React / TypeScript)

#### 1. Upload Video Form
- **File**: `hermes-web/src/components/UploadVideoForm.tsx`
- New form component for uploading YouTube videos
- Features:
  - Input for YouTube URL
  - Language and topic selection
  - Loading state during processing
  - Automatic redirect to VideoTranslateView on success

#### 2. Updated Upload View with Tabs
- **File**: `hermes-web/src/views/Upload/UploadView.tsx`
- Added tab navigation to switch between:
  - "Upload Text" (existing functionality)
  - "Upload YouTube Video" (new functionality)

#### 3. Video Translate View
- **File**: `hermes-web/src/views/Translate/VideoTranslateView.tsx`
- New view for video articles with:
  - Embedded YouTube player
  - Video title display
  - Scrollable caption list with timestamps
  - Placeholder for translation functionality (to be implemented later)
  - Color-coded legend for translation states

#### 4. Updated Files
- `ArticleTemplateAPI.ts` - Added `uploadVideo` method and updated DTOs
- `App.tsx` - Added route for `/video-translate/:articleTemplateID`
- `ArticleTemplatesView.tsx` - Updated to:
  - Display video articles with video icon/indicator
  - Redirect to VideoTranslateView when clicking video article cards
  - Pass video metadata to article cards

## Prerequisites

Before running this feature, ensure you have:

1. **yt-dlp installed** on the server where hermes-api runs:
   ```bash
   # On Windows (using winget)
   winget install yt-dlp
   
   # Or using pip
   pip install yt-dlp
   
   # On Linux/Mac
   brew install yt-dlp
   # or
   sudo curl -L https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp -o /usr/local/bin/yt-dlp
   sudo chmod a+rx /usr/local/bin/yt-dlp
   ```

2. **Run the database migration**:
   ```sql
   -- Execute the migration script
   SOURCE hermes-api/Shell/Scripts/0020_ADD_VIDEO_SUPPORT.sql;
   ```

## How to Use

### For Administrators

1. Navigate to **Upload** page
2. Click the **"Upload YouTube Video"** tab
3. Select the video's language
4. Select a topic
5. Paste the YouTube URL (supports formats like):
   - `https://www.youtube.com/watch?v=VIDEO_ID`
   - `https://youtu.be/VIDEO_ID`
   - `https://www.youtube.com/embed/VIDEO_ID`
6. Click **"Upload Video"**
7. Wait for processing (the system will download captions and extract metadata)
8. You'll be redirected to the Video Translate View

### For Users

1. In **Text Storage**, video articles are now displayed alongside text articles
2. Click on a video article card to view it
3. The VideoTranslateView shows:
   - The embedded YouTube video
   - Original captions with timestamps
   - Translation interface (to be fully implemented later)

### For Adding Video Articles to Rooms

1. Video articles appear in the Article Templates list
2. Click "Add to room" on a video article card
3. The video article is added to the room
4. Users in the room can translate the video captions

## Technical Details

### Caption Processing

The system:
1. Downloads both manual and auto-generated captions in VTT format
2. Prioritizes manual captions over auto-generated ones
3. Attempts to find captions in the video's original language
4. Falls back to any available caption track if original language is not found
5. Parses VTT files to extract:
   - Text content (cleaned of VTT tags)
   - Start timestamp in milliseconds
   - End timestamp in milliseconds

### Data Model

**Video Article Template**:
```typescript
{
  articleTemplateID: string,
  isVideo: boolean,
  videoURL: string,
  title: [{ text: string, startTimeMs?: number, endTimeMs?: number }],
  text: [{ text: string, startTimeMs: number, endTimeMs: number }],
  // ... other fields
}
```

### Future Enhancements (Not Implemented Yet)

1. **Video-Caption Sync**: Highlight captions in the article as they appear in the video
2. **Translation Interface**: Full translation UI in VideoTranslateView
3. **Video Player Controls**: Seek to specific captions by clicking them
4. **Auto-play**: Start video at specific caption timestamp
5. **Caption Editing**: Allow admins to edit captions if needed

## Error Handling

The system handles:
- Invalid YouTube URLs → Error message displayed
- Videos without captions → Error message displayed
- yt-dlp not installed → Error message displayed
- Network errors during download → Error message displayed
- Temporary file cleanup failures → Ignored (logged internally)

## Files Created

### Backend
1. `hermes-api/Shell/Scripts/0020_ADD_VIDEO_SUPPORT.sql`
2. `hermes-api/Core/Services/YouTubeService.cs`
3. `hermes-api/Core/Aggregates/ArticleTemplate/Commands/ArticleTemplateUploadVideoCommand.cs`
4. `hermes-api/Core/Aggregates/ArticleTemplate/Events/ArticleTemplateVideoUploadedEvent.cs`
5. `hermes-worker/Core/Model/Events/ArticleTemplate/ArticleTemplateVideoUploadedEvent.cs`

### Frontend
1. `hermes-web/src/components/UploadVideoForm.tsx`
2. `hermes-web/src/views/Translate/VideoTranslateView.tsx`

### Files Modified

**Backend**:
- `ArticleTemplateCommands.cs`
- `ArticleTemplateEvents.cs`
- `ArticleTemplatesController.cs`
- `ArticleTemplateDTO.cs`
- `ArticleTemplatesDTO.cs`
- `ArticleTemplateQuery.cs`
- `ArticleTemplateRepository.cs`
- `ArticleTemplateSentenceRepository.cs`

**Frontend**:
- `UploadView.tsx`
- `ArticleTemplateAPI.ts`
- `App.tsx`
- `ArticleTemplatesView.tsx`

## Testing Checklist

- [ ] Install yt-dlp on the server
- [ ] Run database migration
- [ ] Restart hermes-api and hermes-worker services
- [ ] Rebuild frontend (`npm run build`)
- [ ] Test uploading a YouTube video with manual captions
- [ ] Test uploading a YouTube video with auto-generated captions
- [ ] Verify video appears in Text Storage
- [ ] Verify clicking video card opens VideoTranslateView
- [ ] Verify YouTube player loads correctly
- [ ] Verify captions display with timestamps
- [ ] Test adding video article to a room
- [ ] Verify error handling for invalid URLs

## Notes

- The backend uses async processing for video uploads since yt-dlp operations can take a few seconds
- The frontend shows a loading state during video processing
- Temporary files are stored in system temp directory and automatically cleaned up
- Video thumbnails are fetched from YouTube and stored in the `photoURL` field
- The system respects YouTube's terms of service by only downloading metadata and captions, not the video file itself




